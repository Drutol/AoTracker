using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Infrastructure.Util
{
    public static class SharedUtil
    {
        public static string TimeDiffToString(TimeSpan diff)
        {
            var changedDiff = string.Empty;
            if (diff.TotalDays > 1)
                changedDiff += $"{diff.Days}d ";
            if (diff.TotalHours > 1)
                changedDiff += $"{diff.Hours}h ";
            changedDiff += $"{diff.Minutes}m";
            return changedDiff;
        }
    }
}
