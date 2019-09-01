using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Interfaces;
using AoTracker.Android.BackgroundWork;
using AoTracker.Android.Utils;
using AoTracker.Interfaces;
using AoTracker.Interfaces.Adapters;
using AoTracker.Resources;
using Microsoft.Extensions.Logging;

namespace AoTracker.Android.Adapters
{
    public class BackgroundJobsManager : IInitializable
    {
        private const int FeedUpdateJobId = 1;
        private readonly IContextProvider _contextProvider;
        private readonly ISnackbarProvider _snackbarProvider;
        private readonly ILogger<BackgroundJobsManager> _logger;

        public BackgroundJobsManager(
            IContextProvider contextProvider,
            ISnackbarProvider snackbarProvider,
            ILogger<BackgroundJobsManager> logger)
        {
            _contextProvider = contextProvider;
            _snackbarProvider = snackbarProvider;
            _logger = logger;
        }

        public Task Initialize()
        {
            var jobScheduler =
                (JobScheduler) _contextProvider.CurrentContext.GetSystemService(Context.JobSchedulerService);


            if (jobScheduler.GetPendingJob(FeedUpdateJobId) == null || true)
            {
                var builder =
                    _contextProvider.CurrentContext
                        .CreateJobBuilderUsingJobId<FeedUpdateService>(FeedUpdateJobId);

                builder.SetPeriodic(
                    (int) TimeSpan.FromHours(1).TotalMilliseconds,
                    (int) TimeSpan.FromHours(1).TotalMilliseconds);

                builder.SetRequiredNetwork(new NetworkRequest.Builder()
                    .AddTransportType(TransportType.Wifi)
                    .AddTransportType(TransportType.Cellular)
                    .AddCapability(NetCapability.Internet)
                    .Build());

                var result = jobScheduler.Schedule(builder.Build());

                if(result == JobScheduler.ResultSuccess)
                {
                    _logger.LogInformation("Successfully scheduled feed update job.");
                }
                else
                {
                    _logger.LogInformation("Failed to schedule feed update job.");
                    _snackbarProvider.ShowToast(AppResources.Error_FailedToCreateFeedUpdateJob);
                }
            }

            return Task.CompletedTask;
        }
    }
}