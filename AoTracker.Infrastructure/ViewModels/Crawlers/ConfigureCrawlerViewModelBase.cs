using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Utilities.Shared;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Messaging;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Interfaces.Adapters;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Crawlers
{
    public abstract class ConfigureCrawlerViewModelBase<TParameters> : ViewModelBase
        where TParameters : class, ICrawlerSourceParameters, new()
    {
        private readonly INavigationManager<PageIndex> _navigationManager;
        private readonly ISnackbarProvider _snackbarProvider;
        private ConfigureCrawlerPageNavArgs _navArgs;
        private string _searchQueryInput;
        private double _costPercentageIncrease;
        private double _costOffsetIncrease;

        public SmartObservableCollection<string> ExcludedKeywords { get; set; } = new SmartObservableCollection<string>();

        protected abstract CrawlerDomain Domain { get; }
        protected abstract TParameters FillInParameters(TParameters parameters);
        protected abstract void InitParameters(TParameters parameters);

        public ConfigureCrawlerViewModelBase(
            INavigationManager<PageIndex> navigationManager,
            ISnackbarProvider snackbarProvider)
        {
            _navigationManager = navigationManager;
            _snackbarProvider = snackbarProvider;
        }

        public void NavigatedTo(ConfigureCrawlerPageNavArgs navArgs)
        {
            MessengerInstance.Send(ToolbarRequestMessage.ShowSaveButton);
            MessengerInstance.Register<ToolbarActionMessage>(this, OntoolbarAction);

            _navArgs = navArgs;
            PageTitle = string.Format(AppResources.PageTitle_ConfigureCrawlers, Domain.ToString());

            if (!navArgs.ConfigureNew)
            {
                var descriptor = navArgs.DescriptorToEdit.CrawlerSourceParameters;

                SearchQueryInput = descriptor.SearchQuery;
                CostOffsetIncrease = descriptor.OffsetIncrease;
                CostPercentageIncrease = descriptor.PercentageIncrease;
                ExcludedKeywords.AddRange(descriptor.ExcludedKeywords ?? Enumerable.Empty<string>());

                InitParameters(descriptor as TParameters);
            }
        }

        private void OntoolbarAction(ToolbarActionMessage message)
        {
            if (message == ToolbarActionMessage.ClickedSaveButton)
            {
                SaveCommand.Execute(null);
            }
        }

        public void NavigatedFrom()
        {
            MessengerInstance.Send(ToolbarRequestMessage.ResetToolbar);
            MessengerInstance.Unregister<ToolbarActionMessage>(this, OntoolbarAction);
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

        public RelayCommand<string> AddExcludedKeywordCommand => new RelayCommand<string>(keyword =>
        {
            if (!string.IsNullOrEmpty(keyword) && !ExcludedKeywords.Contains(keyword))
            {
                if (ExcludedKeywords.Count >= 5)
                {
                    _snackbarProvider.ShowToast(AppResources.Toast_MaxCountOfExcludedKeywordsReached);
                    return;
                }
                ExcludedKeywords.Add(keyword);
            }
        });  
        
        public RelayCommand<string> RemoveExcludedKeywordCommand => new RelayCommand<string>(keyword =>
        {
            ExcludedKeywords.Remove(keyword);
        });

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
                ExcludedKeywords = ExcludedKeywords.ToList()
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
