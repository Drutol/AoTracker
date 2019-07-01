using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Infrastructure.Models.NavArgs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AoTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigureSurugayaCrawlerPage
    {
        private readonly ConfigureCrawlerPageNavArgs _navArgs;

        public ConfigureSurugayaCrawlerPage(ConfigureCrawlerPageNavArgs navArgs)
        {
            _navArgs = navArgs;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ViewModel.NavigatedTo(_navArgs);
        }
    }
}