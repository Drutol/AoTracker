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
using AoTracker.UWP.Utils;
using Autofac;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AoTracker.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            App.NavigationManager = new UwpNavigationManager(new Dictionary<NavigationStack, Frame>
            {
                {NavigationStack.MainStack, MainFrame},
                {NavigationStack.OffStack, OffFrame},

            });
            App.DialogManager =
                new CustomDialogsManager<DialogIndex>(
                    new Dictionary<DialogIndex, ICustomDialogProvider>
                    {
                        {DialogIndex.ChangelogDialog, new OneshotCustomDialogProvider<ChangelogDialog>()},
                    },
                    new DependencyResolver());

            ViewModel = ResourceLocator.ObtainScope().Resolve<MainViewModel>();
            DataContext = ViewModel;
            ViewModel.Initialize();
        }

        class UwpNavigationManager : NavigationManager<PageIndex>
        {
            private readonly Dictionary<NavigationStack, Frame> _frames;

            public UwpNavigationManager(Dictionary<NavigationStack, Frame> frames) : base(
                null,
                new DependencyResolver(),
                new UwpStackResolver())
            {
                _frames = frames;
            }

            public override void CommitPageTransaction(NavigationPageBase page)
            {
                var frame = _frames[((PageIndex) (page.PageIdentifier)).GetRelevantStack()];
                frame.Navigate(page.GetType());
            }
        }

        class UwpStackResolver : IStackResolver<NavigationPageBase, PageIndex>
        {
            private Dictionary<NavigationStack, TaggedStack<BackstackEntry<NavigationPageBase>>> _stacks = new Dictionary<NavigationStack, TaggedStack<BackstackEntry<NavigationPageBase>>>
            {
                {NavigationStack.MainStack, new TaggedStack<BackstackEntry<NavigationPageBase>>() },
                {NavigationStack.OffStack, new TaggedStack<BackstackEntry<NavigationPageBase>>() },
            };

            public TaggedStack<BackstackEntry<NavigationPageBase>> ResolveStackForIdentifier(PageIndex identifier)
            {
                return _stacks[identifier.GetRelevantStack()];
            }

            public TaggedStack<BackstackEntry<NavigationPageBase>> ResolveStackForTag(Enum tag)
            {
                return _stacks[((PageIndex) tag).GetRelevantStack()];
            }
        }

        private void NavigationView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if(args.SelectedItem is NavigationViewItem navItem)
                ViewModel.SelectHamburgerItemCommand.Execute(ViewModel.SettingsButtonViewModel);
            else
                ViewModel.SelectHamburgerItemCommand.Execute(args.SelectedItem);
        }
    }
}
