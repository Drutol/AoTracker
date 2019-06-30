using System;
using System.ComponentModel;
using AoTracker.Infrastructure.Statics;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Interfaces;
using AoTracker.Navigation;
using AoTracker.Views;
using AoTracker.Views.Main;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Autofac;
using Xamarin.Forms.Internals;

namespace AoTracker
{
    public partial class App : Application
    {
        private RootPage _rootPage;
        public MainViewModel ViewModel { get; set; }

        internal static App Instance { get; private set; }

        public App(Action<ContainerBuilder> dependenciesRegistration)
        {
            Instance = this;
            InitializeComponent();

            var resolver = AppInitializationRoutines.InitializeDependencyInjection(WrappedRegistrations);
            DependencyResolver.ResolveUsing(resolver);
            
            _rootPage = new RootPage();
            MainPage = _rootPage;

            void WrappedRegistrations(ContainerBuilder builder)
            {
                dependenciesRegistration(builder);

                builder.RegisterType<NavigationManager>().As<INavigationManager>().SingleInstance();
                builder.Register(context => _rootPage.Detail.Navigation).As<INavigation>();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
