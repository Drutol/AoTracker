using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Infrastructure.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AoTracker.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPageMaster
    {
        public MainViewModel ViewModel { get; set; }

        public RootPageMaster()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ViewModel = DependencyService.Resolve<MainViewModel>();
            BindingContext = ViewModel;
            ViewModel.Initialize();
        }
    }
}