using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Interfaces
{
    public interface IFeedUpdateBackgroundServiceManager
    {
        void Schedule();
        void Unschedule();
    }
}
