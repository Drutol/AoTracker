using System;
using System.ComponentModel;
using AoTracker.Infrastructure.Statics;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Interfaces;
using AoTracker.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Autofac;
using Xamarin.Forms.Internals;

namespace AoTracker
{
    public partial class App : Application
    {
        public MainViewModel ViewModel { get; set; }

        internal static App Instance { get; private set; }

        public App(Action<ContainerBuilder> dependenciesRegistration)
        {
            Instance = this;
            InitializeComponent();

            var resolver = AppInitializationRoutines.InitializeDependencyInjection(WrappedRegistrations);
            DependencyResolver.ResolveUsing(resolver);

            ViewModel = DependencyService.Resolve<MainViewModel>();
            ViewModel.Initialize();

            void WrappedRegistrations(ContainerBuilder builder)
            {
                dependenciesRegistration(builder);

                builder.RegisterType<OuterNavigationManager>().As<IOuterNavigationManager>().SingleInstance();
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
