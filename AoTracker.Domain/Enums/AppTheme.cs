using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Domain.Enums
{
    [Flags]
    public enum AppTheme
    {
        Dark = 1,
        Light = 2,

        Orange = 1 << 3,
        Lime = 1 << 4,
        SkyBlue = 1 << 5,
        Purple = 1 << 6,
        Red = 1 << 7,
        Cyan = 1 << 8,
    }
}
