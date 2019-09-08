using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using AoTracker.Crawlers.Surugaya;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.UWP.Behaviours;
using AoTracker.UWP.Behaviours.Interfaces;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AoTracker.UWP.UserControls
{
    [ContentProperty(Name = nameof(CustomContent))]
    public sealed partial class CrawlerItem : IElementWithNestedBindings
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string NestedPropertyName { get; } = nameof(CrawlerDescriptorViewModel.CrawlerSourceParameters);

        public static readonly DependencyProperty CustomContentProperty = DependencyProperty.Register(
            "CustomContent", typeof(object), typeof(CrawlerItem), new PropertyMetadata(default(object)));

        public object CustomContent
        {
            get => (object)GetValue(CustomContentProperty);
            set => SetValue(CustomContentProperty, value);
        }

        public CrawlerDescriptorViewModel ViewModel { get; private set; }

        public SurugayaSourceParameters SurugayaSourceParameters { get; set; }

        public CrawlerItem()
        {
            this.InitializeComponent();
            DataContextChanged += (sender, args) =>
            {
                if(DataContext != null)
                {
                    if(ViewModel != null)
                        ViewModel.PropertyChanged -= PropertyChanged;
                    ViewModel = (CrawlerDescriptorViewModel)DataContext;
                    ViewModel.PropertyChanged += PropertyChanged;
                    UpdateSurugayaParams();
                    Bindings.Update();
                }
            };
        }

        public void UpdateBindings()
        {
            UpdateSurugayaParams();
            Bindings.Update();
        }

        private void UpdateSurugayaParams()
        {
            if (ViewModel.CrawlerSourceParameters is SurugayaSourceParameters param)
                SurugayaSourceParameters = param;
        }
    }
}
