using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AoTracker.UWP.Converters
{
    public class ItemIndexToIconConverter : ItemIndexConverter
    {
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            var index = (int)base.Convert(value, targetType, parameter, language) + 1;
            if (index == 1)
                return "\uf3a6";
            if (index == 2)
                return "\uf3a9";
            if (index == 3)
                return "\uf3ac";
            if (index == 4)
                return "\uf3af";
            if (index == 5)
                return "\uf3b2";
            if (index == 6)
                return "\uf3b5";
            if (index == 7)
                return "\uf3b8";
            if (index == 8)
                return "\uf3bb";
            if (index == 9)
                return "\uf3be";

            return string.Empty;
        }
    }
}
