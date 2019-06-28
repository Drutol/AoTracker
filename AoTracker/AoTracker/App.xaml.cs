using System;
using AoTracker.Infrastructure.Statics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AoTracker.Services;
using AoTracker.Views;
using Xamarin.Forms.Internals;

namespace AoTracker
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            var resolver = AppInitializationRoutines.InitializeDependencyInjection();
            DependencyResolver.ResolveUsing(resolver);
            MainPage = new AppShell();
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
