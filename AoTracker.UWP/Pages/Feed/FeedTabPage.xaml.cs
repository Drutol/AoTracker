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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AoLibs.Navigation.UWP.Pages;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.ViewModels.Feed;
using GalaSoft.MvvmLight.Messaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AoTracker.UWP.Pages.Feed
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedTabPage 
    {
        public static readonly DependencyProperty FeedTabEntryProperty = DependencyProperty.Register(
            "FeedTabEntry", typeof(FeedTabEntry), typeof(FeedTabPage), new PropertyMetadata(default(FeedTabEntry), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = (FeedTabPage)d;
            if (e.NewValue != null)
            {
                Messenger.Default.Send(new FeedPage.NewFeedTabViewModelMessage
                {
                    FeedTabEntry = (FeedTabEntry)e.NewValue,
                    FeedTabViewModel = page
                });
            }
        }

        public FeedTabEntry FeedTabEntry
        {
            get => (FeedTabEntry) GetValue(FeedTabEntryProperty);
            set => SetValue(FeedTabEntryProperty, value);
        }

        public FeedTabPage()
        {
            this.InitializeComponent();
        }

        public override void NavigatedTo()
        {
            ViewModel.TabEntry = FeedTabEntry;
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }
    }

    public class FeedTabPageBase : PageBase<FeedTabViewModel>
    {
        
    }
}
