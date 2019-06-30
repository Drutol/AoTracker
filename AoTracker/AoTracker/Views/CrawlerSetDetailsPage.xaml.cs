using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Domain.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AoTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrawlerSetDetailsPage 
    {
        public CrawlerSet CrawlerSet { get; }

        public CrawlerSetDetailsPage(CrawlerSet crawlerSet)
        {
            CrawlerSet = crawlerSet;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ViewModel.NavigatedTo(CrawlerSet);
        }
    }
}