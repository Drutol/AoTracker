using System;
using System.Collections.Generic;
using System.Diagnostics;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AoLibs.Dialogs.Android;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Android.Dialogs;
using AoTracker.Android.Themes;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Messaging;
using AoTracker.Infrastructure.Statics;
using AoTracker.Infrastructure.ViewModels;
using Autofac;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Logging;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using Toolbar = global::Android.Support.V7.Widget.Toolbar;

namespace AoTracker.Android.Activities
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AoTracker.Splashscreen",
        Icon = "@mipmap/ic_launcher",
        RoundIcon = "@mipmap/ic_launcher_round",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleInstance,
        TaskAffinity = "",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.KeyboardHidden | ConfigChanges.Keyboard)]
    public partial class MainActivity : AppCompatActivity
    {
        private static MainViewModel ViewModel { get; set; }
        private static ILogger<MainActivity> _logger;
        private static bool _initialized;

        public static Activity Instance { get; set; }

        public List<Binding> Bindings { get; } = new List<Binding>();

        public MainActivity()
        {
            Instance = this;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.ApplyTheme();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            AppCenter.Start("c0a878f9-4b3b-4c60-b56d-41e237fbf515", typeof(Analytics), typeof(Crashes));
            SetContentView(Resource.Layout.activity_main);

            if (!_initialized)
            {
                App.NavigationManager = new NavigationManager<PageIndex>(
                    SupportFragmentManager,
                    RootView,
                    new ViewModelResolver());
                App.DialogManager = new CustomDialogsManager<DialogIndex>(
                    SupportFragmentManager,
                    new Dictionary<DialogIndex, ICustomDialogProvider>
                    {
                        {DialogIndex.ChangelogDialog, new OneshotCustomDialogProvider<ChangelogDialog>()}
                    },
                    new ViewModelResolver());

                AppInitializationRoutines.InitializeDependencies();


                using (var scope = ResourceLocator.ObtainScope())
                {
                    ViewModel = scope.Resolve<MainViewModel>();
                    _logger = scope.Resolve<ILogger<MainActivity>>();
                }
                SetSupportActionBar(Toolbar);
                InitDrawer();
                ViewModel.Initialize();

                _initialized = true;
            }
            else
            {
                App.NavigationManager.RestoreState(SupportFragmentManager, RootView);
                App.DialogManager.ChangeFragmentManager(SupportFragmentManager);
                SetSupportActionBar(Toolbar);
                InitDrawer();
            }


        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_hamburgerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);

        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
        }

        protected override void OnResume()
        {
            _logger?.LogInformation("App resumed.");
            base.OnResume();
        }

        protected override void OnRestart()
        {
            _logger.LogInformation("Restarting");
            base.OnRestart();
        }

        public override void OnBackPressed()
        {
            if (!App.NavigationManager.OnBackRequested())
            {
                _logger.LogInformation("Moving task to back.");
                MoveTaskToBack(true);
            }
        }

        #region Views

        private Toolbar _toolbar;
        private FrameLayout _rootView;
        private RecyclerView _navigationRecyclerView;
        private FrameLayout _settingsNavButton;
        private NavigationView _navigationView;
        private DrawerLayout _drawerLayout;

        public Toolbar Toolbar => _toolbar ?? (_toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar));
        public FrameLayout RootView => _rootView ?? (_rootView = FindViewById<FrameLayout>(Resource.Id.RootView));

        public RecyclerView NavigationRecyclerView => _navigationRecyclerView ??
                                                      (_navigationRecyclerView =
                                                          FindViewById<RecyclerView>(Resource.Id.NavigationRecyclerView)
                                                      );

        public FrameLayout SettingsNavButton => _settingsNavButton ??
                                                (_settingsNavButton =
                                                    FindViewById<FrameLayout>(Resource.Id.SettingsNavButton));

        public NavigationView NavigationView => _navigationView ??
                                                (_navigationView =
                                                    FindViewById<NavigationView>(Resource.Id.NavigationView));

        public DrawerLayout DrawerLayout =>
            _drawerLayout ?? (_drawerLayout = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout));

        #endregion

        private class ViewModelResolver : IViewModelResolver, ICustomDialogViewModelResolver
        {
            TViewModel IViewModelResolver.Resolve<TViewModel>()
            {
                return Resolve<TViewModel>();
            }

            TViewModel ICustomDialogViewModelResolver.Resolve<TViewModel>()
            {
                return Resolve<TViewModel>();
            }

            private TViewModel Resolve<TViewModel>()
            {
                try
                {
                    return ResourceLocator.CurrentScope.Resolve<TViewModel>();
                }
                catch (Exception e)
                {
                    Debugger.Break();
                    throw;
                }
            }
        }
    }
}