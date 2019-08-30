using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Android.Activities;
using AoTracker.Android.Adapters;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Statics;
using AoTracker.Interfaces.Adapters;
using Autofac;
using FFImageLoading;
using Microsoft.Extensions.Logging;
using ModernHttpClient;
using Newtonsoft.Json;

namespace AoTracker.Android
{
    [Application]
    public class App : Application
    {
        public static App Current { get; private set; }
        public static INavigationManager<PageIndex> NavigationManager { get; set; }

        public App(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
            Current = this;
        }

        public override void OnCreate()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            ImageService.Instance.Config.HttpClient = new HttpClient(new NativeMessageHandler(
                throwOnCaptiveNetwork: false,
                new TLSConfig
                {
                    DangerousAcceptAnyServerCertificateValidator = true,
                    DangerousAllowInsecureHTTPLoads = true
                }) {AllowAutoRedirect = true});
            AppInitializationRoutines.InitializeDependencyInjection(DependenciesRegistration);
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
            containerBuilder.RegisterType<PickerAdapter>().As<IPickerAdapter>().SingleInstance();
            containerBuilder.RegisterType<ContextProvider>().As<IContextProvider>().SingleInstance();
            containerBuilder.RegisterType<PhotoPickerAdapter>().As<IPhotoPickerAdapter>().SingleInstance();
            containerBuilder.RegisterType<PhoneCallAdapter>().As<IPhoneCallAdapter>().SingleInstance();
            containerBuilder.RegisterType<SnackbarProvider>().As<ISnackbarProvider>().SingleInstance();
            containerBuilder.RegisterType<AndroidLoggerProvider>().As<ILoggerProvider>().SingleInstance();

            containerBuilder.RegisterType<HttpClientProvider>().As<IHttpClientProvider>();

            containerBuilder
                .Register(context => MainActivity.Instance);

            containerBuilder.Register(ctx => NavigationManager).As<INavigationManager<PageIndex>>();
        }

        private class ContextProvider : IContextProvider
        {
            public Activity CurrentContext => MainActivity.Instance;
        }
    }
}