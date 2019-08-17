using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Crawlers
{
    public abstract class ConfigureCrawlerViewModelBase<TParameters> : ViewModelBase
        where TParameters : class, ICrawlerSourceParameters, new()
    {
        private readonly INavigationManager<PageIndex> _navigationManager;
        private ConfigureCrawlerPageNavArgs _navArgs;
        private string _searchQueryInput;
        private double _costPercentageIncrease;
        private double _costOffsetIncrease;

        protected abstract CrawlerDomain Domain { get; }
        protected abstract TParameters FillInParameters(TParameters parameters);
        protected abstract void InitParameters(TParameters parameters);

        public ConfigureCrawlerViewModelBase(INavigationManager<PageIndex> navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void NavigatedTo(ConfigureCrawlerPageNavArgs navArgs)
        {
            _navArgs = navArgs;
            Title = string.Format(AppResources.PageTitle_ConfigureCrawlers, Domain.ToString());

            if (!navArgs.ConfigureNew)
            {
                var descriptor = navArgs.DescriptorToEdit.CrawlerSourceParameters;

                SearchQueryInput = descriptor.SearchQuery;
                CostOffsetIncrease = descriptor.OffsetIncrease;
                CostPercentageIncrease = descriptor.PercentageIncrease;

                InitParameters(descriptor as TParameters);
            }
        }

        public string SearchQueryInput
        {
            get => _searchQueryInput;
            set => Set(ref _searchQueryInput, value);
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

            var parameters = FillInParameters(new TParameters
            {
                SearchQuery = SearchQueryInput,
                OffsetIncrease = CostOffsetIncrease,
                PercentageIncrease = CostPercentageIncrease,
            });

            switch (resultMessage.Action)
            {
                case ConfigureCrawlerResultMessage.ActionType.Add:
                    resultMessage.CrawlerDescriptor = new CrawlerDescriptor
                    {
                        CrawlerDomain = Domain,
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
