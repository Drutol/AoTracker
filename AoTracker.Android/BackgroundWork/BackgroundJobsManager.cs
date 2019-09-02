using System;
using System.Threading.Tasks;
using Android.App.Job;
using Android.Content;
using Android.Net;
using AoLibs.Adapters.Android.Interfaces;
using AoTracker.Android.Utils;
using AoTracker.Interfaces;
using AoTracker.Interfaces.Adapters;
using AoTracker.Resources;
using Microsoft.Extensions.Logging;

namespace AoTracker.Android.BackgroundWork
{
    public class BackgroundJobsManager : IFeedUpdateBackgroundServiceManager, IInitializable
    {
        private const int FeedUpdateJobId = 1;
        private readonly IContextProvider _contextProvider;
        private readonly ISettings _settings;
        private readonly ISnackbarProvider _snackbarProvider;
        private readonly ILogger<BackgroundJobsManager> _logger;

        public BackgroundJobsManager(
            IContextProvider contextProvider,
            ISettings settings,
            ISnackbarProvider snackbarProvider,
            ILogger<BackgroundJobsManager> logger)
        {
            _contextProvider = contextProvider;
            _settings = settings;
            _snackbarProvider = snackbarProvider;
            _logger = logger;
        }

        public Task Initialize()
        {
            var jobScheduler =
                (JobScheduler) _contextProvider.CurrentContext.GetSystemService(Context.JobSchedulerService);

            if (!_settings.FeedUpdateJobScheduled)
            {
                var job = _contextProvider.CurrentContext
                    .CreateJobBuilderUsingJobId<FeedUpdateService>(FeedUpdateJobId)
                    .SetRequiredNetwork(new NetworkRequest.Builder()
                        .AddTransportType(TransportType.Wifi)
                        .AddTransportType(TransportType.Cellular)
                        .AddCapability(NetCapability.Internet)
                        .Build())
                    .SetPeriodic(
                        (int) TimeSpan.FromMinutes(15).TotalMilliseconds + 1)
                    .SetPersisted(true)
                    .Build();

                var result = jobScheduler.Schedule(job);

                if (result == JobScheduler.ResultSuccess)
                {
                    _settings.FeedUpdateJobScheduled = true;
                    _logger.LogInformation("Successfully scheduled feed update job.");
                }
                else
                {
                    _settings.FeedUpdateJobScheduled = false;
                    _logger.LogInformation("Failed to schedule feed update job.");
                    _snackbarProvider.ShowToast(AppResources.Error_FailedToCreateFeedUpdateJob);
                }
            }

            return Task.CompletedTask;
        }

        public void Schedule()
        {
            Initialize();
        }

        public void Unschedule()
        {
            var jobScheduler =
                (JobScheduler)_contextProvider.CurrentContext.GetSystemService(Context.JobSchedulerService);

            jobScheduler.Cancel(FeedUpdateJobId);

            _settings.FeedUpdateJobScheduled = false;
        }
    }
}