using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;

namespace AoTracker.Interfaces
{
    public interface INavigationManager
    {
        void NavigateRoot(PageIndex navigationPageIndex, object parameter = null);

        void PushPage(PageIndex navigationPageIndex, object parameter = null);
    }
}
