﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Adapters.UWP;
using AoLibs.Dialogs.Android;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.UWP;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Statics;
using AoTracker.Interfaces;
using AoTracker.Interfaces.Adapters;
using AoTracker.UWP.Adapters;
using AoTracker.UWP.BackgroundWork;
using AoTracker.UWP.Utils;
using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AoTracker.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static MainPage.UwpNavigationManager NavigationManager { get; set; }
        public static CustomDialogsManager<DialogIndex> DialogManager { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            DataCache.JsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            AppInitializationRoutines.InitializeDependencyInjection(DependenciesRegistration);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        private void DependenciesRegistration(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ClipboardProvider>().As<IClipboardProvider>().SingleInstance();
            containerBuilder.RegisterType<DispatcherAdapter>().As<IDispatcherAdapter>().SingleInstance();
            containerBuilder.RegisterType<FileStorageProvider>().As<IFileStorageProvider>().SingleInstance();
            containerBuilder.RegisterType<MessageBoxProvider>().As<IMessageBoxProvider>().SingleInstance();
            containerBuilder.RegisterType<SettingsProvider>().As<ISettingsProvider>().SingleInstance();
            containerBuilder.RegisterType<UriLauncherAdapter>().As<IUriLauncherAdapter>().SingleInstance();
            containerBuilder.RegisterType<VersionProvider>().As<IVersionProvider>().SingleInstance();
            containerBuilder.RegisterType<PhoneCallAdapter>().As<IPhoneCallAdapter>().SingleInstance();
            containerBuilder.RegisterType<SnackbarProvider>().As<ISnackbarProvider>().SingleInstance();
            //containerBuilder.RegisterType<AndroidLoggerProvider>().As<ILoggerProvider>().SingleInstance();
            containerBuilder.RegisterType<BackgroundJobsManager>().As<IInitializable>().As<IFeedUpdateBackgroundServiceManager>().SingleInstance();

            containerBuilder.RegisterType<HttpClientProvider>().As<IHttpClientProvider>();

            containerBuilder.Register(ctx => NavigationManager).As<INavigationManager<PageIndex>>();
            containerBuilder.Register(ctx => DialogManager).As<ICustomDialogsManager<DialogIndex>>();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    AppInitializationRoutines.InitializeDependencies();
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
