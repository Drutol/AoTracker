using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Android.Activities;
using AoTracker.Android.Adapters;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Statics;
using AoTracker.Interfaces;
using AoTracker.Interfaces.Adapters;
using AoTracker.Resources;
using Autofac;
using Java.Lang;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace AoTracker.Android.BackgroundWork
{
    [Service(
        Name = "com.drutol.aotracker.FeedUpdateService",
        Permission = "android.permission.BIND_JOB_SERVICE")]
    public class FeedUpdateService : JobService
    {
        private const string FeedUpdateChannel = "FeedUpdateChannel";
        private const int NotificationId = 12345;

        public override bool OnStartJob(JobParameters jobParameters)
        {
            Log.Info(nameof(FeedUpdateService), "Starting feed update job service.");
            Task.Run(async () =>
            {
                try
                {
                    AppInitializationRoutines.InitializeDependenciesForBackground(DependenciesRegistration);
                    using (var scope = ResourceLocator.ObtainScope())
                    {
                        var userDataProvider = scope.Resolve<IUserDataProvider>();
                        await userDataProvider.Initialize();

                        if (!userDataProvider.CrawlingSets.Any())
                            return;

                        var feedProvider = scope.Resolve<IFeedProvider>();
                        var feedHistoryProvider = scope.Resolve<IFeedHistoryProvider>();
                        var cts = new CancellationTokenSource();
                        var tcs = new TaskCompletionSource<bool>();
                        var finishSemaphore = new SemaphoreSlim(1);

                        feedProvider.NewCrawlerBatch += async (sender, batch) =>
                        {
                            try
                            {
                                await finishSemaphore.WaitAsync(cts.Token);

                                if (batch.CrawlerResult.Success)
                                {
                                    if (await feedHistoryProvider.HasAnyChanged(
                                        batch.SetOfOrigin,
                                        batch.CrawlerResult.Results))
                                    {
                                        cts.Cancel();
                                        Log.Info(nameof(FeedUpdateService), "Found new feed content.");
                                        tcs.SetResult(true);
                                    }
                                }
                            }
                            catch (TaskCanceledException)
                            {

                            }
                            finally
                            {
                                finishSemaphore.Release();
                            }
                        };

                        feedProvider.Finished += async (sender, args) =>
                        {
                            if (tcs.Task.IsCompleted)
                                return;
                            try
                            {
                                await finishSemaphore.WaitAsync(cts.Token);
                                if (tcs.Task.IsCompleted)
                                    return;
                                tcs.SetResult(false);
                            }
                            catch
                            {
                                //not important at this point
                            }
                        };

                        feedProvider.StartAggregating(
                            userDataProvider.CrawlingSets.ToList(),
                            cts.Token,
                            true);

                        var result = await tcs.Task;

                        if (result)
                        {
                            var builder = new NotificationCompat.Builder(this, FeedUpdateChannel)
                                .SetSmallIcon(Resource.Drawable.icon_logo_small)
                                .SetContentTitle(AppResources.Notification_FeedUpdate)
                                .SetContentText(AppResources.Notification_FeedUpdate_Description)
                                .SetPriority(NotificationCompat.PriorityDefault);
                            CreateNotificationChannel();

                            var manager = NotificationManager.FromContext(this);
                            manager.Notify(NotificationId, builder.Build());
                        }

                        Log.Info(nameof(FeedUpdateService), "Finishing feed update job service.");
                        JobFinished(jobParameters, true);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(nameof(FeedUpdateService), $"Failed to process feed update notification. {e}");
                }
            });

            return true;
        }

        public override bool OnStopJob(JobParameters jobParameters)
        {
            Log.Info(nameof(FeedUpdateService), "Stopping feed update job service.");
            //on fail we want to reschedule the update
            return true;
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var notificationManager = NotificationManager.FromContext(this);

                if (notificationManager.NotificationChannels.Any(notificationChannel =>
                    notificationChannel.Id == FeedUpdateChannel))
                    return;

                var name = new Java.Lang.String(GetString(Resource.String.Android_NotificationChannel_Name));
                var description = GetString(Resource.String.Android_NotificationChannel_Description);
                NotificationChannel channel =
                    new NotificationChannel(FeedUpdateChannel, name, NotificationImportance.Default)
                    {
                        Description = description
                    };
                // Register the channel with the system; you can't change the importance
                // or other notification behaviors after this

                notificationManager.CreateNotificationChannel(channel);
            }
        }

        private void DependenciesRegistration(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DispatcherAdapter>().As<IDispatcherAdapter>().SingleInstance();
            containerBuilder.RegisterType<FileStorageProvider>().As<IFileStorageProvider>().SingleInstance();
            containerBuilder.RegisterType<SettingsProvider>().As<ISettingsProvider>().SingleInstance();
            containerBuilder.RegisterType<BackgroundContextProvider>().As<IContextProvider>().SingleInstance();
            containerBuilder.RegisterType<AndroidLoggerProvider>().As<ILoggerProvider>().SingleInstance();

            containerBuilder.RegisterType<HttpClientProvider>().As<IHttpClientProvider>();
        }

        class BackgroundContextProvider : IContextProvider
        {
            private readonly JobService _parent;

            public BackgroundContextProvider(JobService parent)
            {
                _parent = parent;
            }

            public Context CurrentContext => _parent;
            public Activity CurrentActivity { get; }
        }
    }
}