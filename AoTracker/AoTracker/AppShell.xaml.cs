using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AoTracker.Domain;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Views;
using Xamarin.Forms;

namespace AoTracker
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public MainViewModel ViewModel { get; }

        public AppShell()
        {
            ViewModel = DependencyService.Resolve<MainViewModel>();
            BindingContext = ViewModel;


            InitializeComponent();
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            DependencyService.Resolve<FeedViewModel>().NavigatedTo();
        }
    }
}
