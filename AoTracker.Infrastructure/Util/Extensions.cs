using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Utilities.Shared;

namespace AoTracker.Infrastructure.Util
{
    public static class Extensions
    {
        public static void PlatformAddRange<T>(this SmartObservableCollection<T> collection, IEnumerable<T> items, PlatformType platformType)
        {
            if(platformType == PlatformType.UWP)
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }
            else
            {
                collection.AddRange(items);
            }
        }
    }
}
