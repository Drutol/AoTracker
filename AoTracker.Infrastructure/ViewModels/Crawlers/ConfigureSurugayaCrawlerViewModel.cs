using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Interfaces;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels
{
    public class ConfigureSurugayaCrawlerViewModel : ViewModelBase
    {
        private readonly INavigationManager _navigationManager;
        private ConfigureCrawlerPageNavArgs _navArgs;
        private string _searchQueryInput;
        private bool _trimJapaneseQuotationMarks;
        private double _costPercentageIncrease;
        private double _costOffsetIncrease;

        public ConfigureSurugayaCrawlerViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void NavigatedTo(ConfigureCrawlerPageNavArgs navArgs)
        {
            _navArgs = navArgs;
            Title = string.Format(AppResources.PageTitle_ConfigureCrawlers, "Suruga-ya");
        }

        public string SearchQueryInput
        {
            get => _searchQueryInput;
            set => Set(ref _searchQueryInput, value);
        }

        public bool TrimJapaneseQuotationMarks
        {
            get => _trimJapaneseQuotationMarks;
            set => Set(ref _trimJapaneseQuotationMarks, value);
        }

        public double CostPercentageIncrease
        {
            get => _costPercentageIncrease;
            set => Set(ref _costPercentageIncrease, value);
        }

        public double CostOffsetIncrease
        {
            get => _costOffsetIncrease;
            set => Set(ref _costOffsetIncrease, value);
        }

        public RelayCommand SaveCommand => new RelayCommand(() =>
        {
            _navArgs.Domain = CrawlerDomain.Surugaya;
            _navArgs.CrawlerSourceParameters = new SurugayaSourceParameters
            {
                SearchQuery = SearchQueryInput,
                TrimJapaneseQuotationMarks = TrimJapaneseQuotationMarks,
                OffsetIncrease = CostOffsetIncrease,
                PercentageIncrease = CostPercentageIncrease
            };

            _navigationManager.GoBack();
        });
    }
}
