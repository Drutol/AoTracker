using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;

namespace AoTracker.Interfaces
{
    public interface ISettings
    {
        bool PassedWelcome { get; set; }
        AppTheme AppTheme { get; set; }
    }
}
