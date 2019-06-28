using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;

namespace AoTracker.Interfaces
{
    public interface IOuterNavigationManager
    {
        void NavigateTo(OuterNavigationPage page);
    }
}
