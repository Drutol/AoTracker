using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;

namespace AoTracker.UWP.MarkupExtensions
{
    [MarkupExtensionReturnType(ReturnType = typeof(Windows.UI.Xaml.Media.FontFamily))]
    public class FontFamilyHelperExtension : MarkupExtension
    {
        public enum FontFamily
        {
            SegoeMdl2Assets,
            MaterialDesignIcons
        }

        public FontFamily Family { get; set; }

        protected override object ProvideValue()
        {
            switch (Family)
            {
                case FontFamily.SegoeMdl2Assets:
                    return new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets");
                case FontFamily.MaterialDesignIcons:
                    return new Windows.UI.Xaml.Media.FontFamily("ms-appx:///Assets/Fonts/mdf.ttf#Material Design Icons");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
