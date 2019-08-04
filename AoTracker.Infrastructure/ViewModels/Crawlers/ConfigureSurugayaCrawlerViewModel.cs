using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Interfaces;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace AoTracker.Infrastructure.ViewModels
{
    public class ConfigureSurugayaCrawlerViewModel : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;
        private ConfigureCrawlerPageNavArgs _navArgs;
        private string _searchQueryInput;
        private bool _trimJapaneseQuotationMarks;
        private double _costPercentageIncrease;
        private double _costOffsetIncrease;

        public ConfigureSurugayaCrawlerViewModel(INavigationManager<PageIndex> navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void NavigatedTo(ConfigureCrawlerPageNavArgs navArgs)
        {
            _navArgs = navArgs;
            Title = string.Format(AppResources.PageTitle_ConfigureCrawlers, "Suruga-ya");

            if (!navArgs.ConfigureNew)
            {
                var surugayaDescriptor = navArgs.DescriptorToEdit.CrawlerSourceParameters as SurugayaSourceParameters;

                SearchQueryInput = surugayaDescriptor.SearchQuery;
                TrimJapaneseQuotationMarks = surugayaDescriptor.TrimJapaneseQuotationMarks;
                CostOffsetIncrease = surugayaDescriptor.OffsetIncrease;
                CostPercentageIncrease = surugayaDescriptor.PercentageIncrease;
            }
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
            var resultMessage = new ConfigureCrawlerResultMessage
            {
                Action = _navArgs.ConfigureNew
                    ? ConfigureCrawlerResultMessage.ActionType.Add
                    : ConfigureCrawlerResultMessage.ActionType.Edit
            };

            var parameters = new SurugayaSourceParameters
            {
                SearchQuery = SearchQueryInput,
                TrimJapaneseQuotationMarks = TrimJapaneseQuotationMarks,
                OffsetIncrease = CostOffsetIncrease,
                PercentageIncrease = CostPercentageIncrease
            };

            switch (resultMessage.Action)
            {
                case ConfigureCrawlerResultMessage.ActionType.Add:
                    resultMessage.CrawlerDescriptor = new CrawlerDescriptor
                    {
                        CrawlerDomain = CrawlerDomain.Surugaya,
                        CrawlerSourceParameters = parameters
                    };
                    break;
                case ConfigureCrawlerResultMessage.ActionType.Edit:
                    _navArgs.DescriptorToEdit.CrawlerSourceParameters = parameters;
                    resultMessage.CrawlerDescriptor = _navArgs.DescriptorToEdit;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            MessengerInstance.Send(resultMessage);

            _navigationManager.GoBack();
        });
    }
}
