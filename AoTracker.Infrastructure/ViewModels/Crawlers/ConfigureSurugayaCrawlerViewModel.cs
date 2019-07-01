using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels
{
    public class ConfigureSurugayaCrawlerViewModel : ViewModelBase
    {
        private readonly INavigationManager _navigationManager;
        private ConfigureCrawlerPageNavArgs _navArgs;
        private string _searchQueryInput;

        public ConfigureSurugayaCrawlerViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void NavigatedTo(ConfigureCrawlerPageNavArgs navArgs)
        {
            _navArgs = navArgs;
        }

        public string SearchQueryInput
        {
            get => _searchQueryInput;
            set => Set(ref _searchQueryInput, value);
        }

        public RelayCommand SaveCommand => new RelayCommand(() =>
        {
            _navArgs.Domain = CrawlerDomain.Surugaya;
            _navArgs.CrawlerSourceParameters = new SurugayaSourceParameters
            {
                SearchQuery = SearchQueryInput
            };

            _navigationManager.GoBack();
        });
    }
}
