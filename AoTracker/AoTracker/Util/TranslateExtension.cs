using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using AoTracker.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AoTracker.Util
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return string.Empty;

            var translation = AppResources.ResourceManager.GetString(Text);

            translation = translation ?? Text;

            return translation;
        }
    }
}
