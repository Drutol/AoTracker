using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AoLibs.Dialogs.Android;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.UWP;
using AoLibs.Navigation.UWP.Pages;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Statics;
using AoTracker.Infrastructure.Util;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.UWP.Dialogs;
using AoTracker.UWP.Models;
using AoTracker.UWP.Models.Messages;
using AoTracker.UWP.Utils;
using Autofac;
using GalaSoft.MvvmLight.Messaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AoTracker.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; set; }

        private bool _hasOffFrameBeenShown;
        private double _offFrameWidthWhenLastHidden;

        public MainPage()
        {
            this.InitializeComponent();

            App.NavigationManager = new UwpNavigationManager(new Dictionary<NavigationStack, Frame>
            {
                {NavigationStack.MainStack, MainFrame},
                {NavigationStack.OffStack, OffFrame},

            }, new UwpStackResolver());

            App.DialogManager =
                new CustomDialogsManager<DialogIndex>(
                    new Dictionary<DialogIndex, ICustomDialogProvider>
                    {
                        {DialogIndex.ChangelogDialog, new OneshotCustomDialogProvider<ChangelogDialog>()},
                    },
                    new DependencyResolver());

            Messenger.Default.Register<NavigationStackUpdateMessage>(this, OnNavigationStackUpdateMessage);
            ViewModel = ResourceLocator.ObtainScope().Resolve<MainViewModel>();
            DataContext = ViewModel;
            ViewModel.Initialize();
        }

        private void OnNavigationStackUpdateMessage(NavigationStackUpdateMessage message)
        {
            if (message.NavigationStack == NavigationStack.OffStack)
            {
                if (message.NavigatedBackToEmpty)
                {
                    OffFrame.Visibility = Visibility.Collapsed;
                    _offFrameWidthWhenLastHidden = OffFrameGridColumn.Width.Value;
                    OffFrameGridColumn.Width = new GridLength(0);
                    App.NavigationManager.Reset(PageIndex.OffStackIdentifier);
                }
                else
                {
                    if (OffFrame.Visibility == Visibility.Collapsed)
                    {
                        OffFrame.Visibility = Visibility.Visible;
                        if (!_hasOffFrameBeenShown)
                        {
                            OffFrameGridColumn.Width = new GridLength(600);
                            _hasOffFrameBeenShown = true;
                        }
                        else
                        {
                            OffFrameGridColumn.Width = new GridLength(_offFrameWidthWhenLastHidden);
                        }
                    }

                    if (message.CanGoBack)
                    {
                        OffBackNavButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        OffBackNavButton.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                NavigationView.IsBackEnabled = message.CanGoBack;
            }


        }

        public class UwpNavigationManager : NavigationManager<PageIndex>
        {
            private readonly Dictionary<NavigationStack, Frame> _frames;
            public UwpStackResolver Resolver { get; }

            public UwpNavigationManager(Dictionary<NavigationStack, Frame> frames, UwpStackResolver resolver) : base(
                null,
                new DependencyResolver(),
                resolver)
            {
                _frames = frames;
                Resolver = resolver;

                WentBack += OnNavigated;
                Navigated += OnNavigated;
                FailedToGoBack += OnFailedToGoBack;
            }

            private void OnFailedToGoBack(object sender, PageIndex e)
            {
                Resolver.OnNavigated(e, true);
            }

            private void OnNavigated(object sender, PageIndex e)
            {
                Resolver.OnNavigated(e, false);
            }

            public override void CommitPageTransaction(NavigationPageBase page)
            {
                var frame = _frames[((PageIndex) (page.PageIdentifier)).GetAssociatedStack()];
                frame.Content = page;
            }
        }

        public class UwpStackResolver : IStackResolver<NavigationPageBase, PageIndex>
        {
            private Dictionary<NavigationStack, TaggedStack<BackstackEntry<NavigationPageBase>>> _stacks = new Dictionary<NavigationStack, TaggedStack<BackstackEntry<NavigationPageBase>>>
            {
                {NavigationStack.MainStack, new TaggedStack<BackstackEntry<NavigationPageBase>>() },
                {NavigationStack.OffStack, new TaggedStack<BackstackEntry<NavigationPageBase>>() },
            };

            public TaggedStack<BackstackEntry<NavigationPageBase>> ResolveStackForIdentifier(PageIndex identifier)
            {
                return _stacks[identifier.GetAssociatedStack()];
            }

            public TaggedStack<BackstackEntry<NavigationPageBase>> ResolveStackForTag(Enum tag)
            {
                return _stacks[((PageIndex) tag).GetAssociatedStack()];
            }

            public void OnNavigated(PageIndex pageIndex, bool isFailedBackNav)
            {
                var stackIdentifier = pageIndex.GetAssociatedStack();
                var stackCount = _stacks[stackIdentifier].Count;

                var message = new NavigationStackUpdateMessage
                {
                    NavigationStack = stackIdentifier,
                    CanGoBack = stackCount > 0,
                    NavigatedBackToEmpty = stackCount <= 0 && isFailedBackNav
                };

                Messenger.Default.Send(message);
            }
        }

        private void NavigationView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if(args.SelectedItem is NavigationViewItem navItem)
                ViewModel.SelectHamburgerItemCommand.Execute(ViewModel.SettingsButtonViewModel);
            else
                ViewModel.SelectHamburgerItemCommand.Execute(args.SelectedItem);
        }

        private void CloseOffPaneButtonOnClick(object sender, RoutedEventArgs e)
        {
            OffFrame.Visibility = Visibility.Collapsed;
            _offFrameWidthWhenLastHidden = OffFrameGridColumn.Width.Value;
            OffFrameGridColumn.Width = new GridLength(0);
            App.NavigationManager.Reset(PageIndex.SettingsIndex);
        }

        private void OffBackNavButton_OnClick(object sender, RoutedEventArgs e)
        {
            App.NavigationManager.GoBack(PageIndex.OffStackIdentifier);
        }

        private void NavigationView_OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            App.NavigationManager.GoBack(PageIndex.MainStackIdentifier);
        }
    }
}
