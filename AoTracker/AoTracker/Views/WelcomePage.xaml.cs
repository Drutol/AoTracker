using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AoTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage
    {
        public WelcomePage()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            ViewModel.NavigatedTo();
        }
    }
}