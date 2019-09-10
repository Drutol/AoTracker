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
using AoLibs.Navigation.UWP.Attributes;
using AoLibs.Navigation.UWP.Pages;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.ViewModels.Feed;
using AoTracker.UWP.Pages.Feed;
using AoTracker.UWP.Utils;
using GalaSoft.MvvmLight.Messaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AoTracker.UWP.Pages
{
    [NavigationPage(PageIndex.Feed)]
    public sealed partial class FeedPage : FeedPageBase
    {
        private readonly Dictionary<FeedTabEntry, FeedTabPage> _childViewModels =
            new Dictionary<FeedTabEntry, FeedTabPage>();

        public class NewFeedTabViewModelMessage
        {
            public FeedTabEntry FeedTabEntry { get; set; }
            public FeedTabPage FeedTabViewModel { get; set; }
        }

        public FeedPage()
        {
            Messenger.Default.Register<NewFeedTabViewModelMessage>(this, OnNewTabMessage);
            this.InitializeComponent();
        }

        private void OnNewTabMessage(NewFeedTabViewModelMessage message)
        {
            _childViewModels[message.FeedTabEntry] = message.FeedTabViewModel;

            if(Pivot.SelectedItem == message.FeedTabEntry)
                message.FeedTabViewModel.NavigatedTo();
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        private void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (FeedTabEntry)e.AddedItems[0];
            if(_childViewModels.TryGetValue(item, out var vm))
                vm.NavigatedTo();
        }
    }

    public class FeedPageBase : CustomPageBase<FeedViewModel>
    {

    }
}
