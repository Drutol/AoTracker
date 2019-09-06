using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AoTracker.UWP.Pages.CrawlerConfigure
{
    [ContentProperty(Name = nameof(CustomContent))]
    public sealed partial class ConfigureCrawlerPageTemplateControl
    {
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(ImageSource), typeof(ConfigureCrawlerPageTemplateControl), new PropertyMetadata(default(ImageSource)));

        public ImageSource ImageSource
        {
            get => (ImageSource) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly DependencyProperty CustomContentProperty = DependencyProperty.Register(
            "CustomContent", typeof(object), typeof(ConfigureCrawlerPageTemplateControl), new PropertyMetadata(default(object)));

        public object CustomContent
        {
            get => (object) GetValue(CustomContentProperty);
            set => SetValue(CustomContentProperty, value);
        }

        public ConfigureCrawlerPageTemplateControl()
        {
            this.InitializeComponent();
        }
    }
}
