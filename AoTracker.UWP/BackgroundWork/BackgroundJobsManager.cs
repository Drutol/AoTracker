using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Interfaces;

namespace AoTracker.UWP.BackgroundWork
{
    public class BackgroundJobsManager : IInitializable, IFeedUpdateBackgroundServiceManager
    {
        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public void Schedule()
        {
            
        }

        public void Unschedule()
        {

        }
    }
}
