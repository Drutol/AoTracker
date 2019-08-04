using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Crawlers.Enums;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.Util;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using AoTracker.Resources;
using Autofac;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels
{
    public class CrawlerSetDetailsViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userDataProvider;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly INavigationManager<PageIndex> _navigationManager;

        private bool _isAddingNew;
        private string _setName;
        private CrawlerSet _currentSet;
        private ObservableCollection<CrawlerDescriptorViewModel> _crawlerDescriptors;

        private List<CrawlerEntry> _crawlerEntries = new List<CrawlerEntry>
        {
            new CrawlerEntry
            {
                CrawlerDomain = CrawlerDomain.Surugaya,
                Title = "Suruga-ya"
            },
            new CrawlerEntry
            {

                CrawlerDomain = CrawlerDomain.Mandarake,
                Title = "Mandarake"
            },
        };

        public CrawlerSetDetailsViewModel(
            IUserDataProvider userDataProvider,
            ILifetimeScope lifetimeScope,
            INavigationManager<PageIndex> navigationManager)
        {
            _userDataProvider = userDataProvider;
            _lifetimeScope = lifetimeScope;
            _navigationManager = navigationManager;
            CrawlerEntries = _crawlerEntries
                .Select(entry => lifetimeScope.TypedResolve<CrawlerEntryViewModel>(entry, this))
                .ToList();

            MessengerInstance.Register<ConfigureCrawlerResultMessage>(this, OnConfigureCrawlerResult);
        }

        private void OnConfigureCrawlerResult(ConfigureCrawlerResultMessage message)
        {
            //navigating back from configure crawler page
            if (message != null)
            {
                if (message.Action == ConfigureCrawlerResultMessage.ActionType.Add)
                {
                    var vm = _lifetimeScope.TypedResolve<CrawlerDescriptorViewModel>(new CrawlerDescriptor
                    {
                        CrawlerDomain = message.CrawlerDescriptor.CrawlerDomain,
                        CrawlerSourceParameters = message.CrawlerDescriptor.CrawlerSourceParameters
                    });
                    CrawlerDescriptors.Add(vm);
                }
                else if(message.Action == ConfigureCrawlerResultMessage.ActionType.Edit)
                {
                    CrawlerDescriptors
                        .First(model => model.BackingModel == message.CrawlerDescriptor)
                        .CrawlerSourceParameters = message.CrawlerDescriptor.CrawlerSourceParameters;
                }
            }
        }

        public void NavigatedTo(CrawlerSetDetailsPageNavArgs navArgs)
        {
            var crawlerSet = navArgs.CrawlerSet;

            if (crawlerSet == _currentSet)
                return;
            
            if (crawlerSet == null)
            {
                Title = _currentSet == null
                    ? AppResources.PageTitle_SetDetails_AddNew
                    : string.Format(AppResources.PageTitle_SetDetails, _currentSet.Name);

                CrawlerDescriptors = new ObservableCollection<CrawlerDescriptorViewModel>();
                IsAddingNew = true;
            }
            else
            {
                _currentSet = crawlerSet;
                Title = string.Format(AppResources.PageTitle_SetDetails, crawlerSet.Name);
                SetName = crawlerSet.Name;
                CrawlerDescriptors = new ObservableCollection<CrawlerDescriptorViewModel>(crawlerSet.Descriptors.Select(
                    descriptor => _lifetimeScope.TypedResolve<CrawlerDescriptorViewModel>(descriptor)));
                IsAddingNew = false;
            }
        }

        public List<CrawlerEntryViewModel> CrawlerEntries { get; }

        public ObservableCollection<CrawlerDescriptorViewModel> CrawlerDescriptors
        {
            get => _crawlerDescriptors;
            set => Set(ref _crawlerDescriptors, value);
        }

        public string SetName
        {
            get => _setName;
            set => Set(ref _setName, value, _ => { RaisePropertyChanged(() => CanSave); });
        }

        public bool IsAddingNew
        {
            get => _isAddingNew;
            set => Set(ref _isAddingNew, value);
        }

        public bool CanSave => true;

        public RelayCommand<CrawlerDescriptor> RemoveDescriptorCommand => new RelayCommand<CrawlerDescriptor>(
            descriptor =>
            {

            });

        public RelayCommand<CrawlerEntryViewModel> AddCrawlerCommand => new RelayCommand<CrawlerEntryViewModel>(entry =>
        {
            var navArgs = new ConfigureCrawlerPageNavArgs
            {
                ConfigureNew = true,
                Domain = entry.BackingModel.CrawlerDomain
            };
            NavigateConfigureDescriptor(navArgs);
        });

        public RelayCommand<CrawlerDescriptorViewModel> SelectCrawlerDescriptorCommand =>
            new RelayCommand<CrawlerDescriptorViewModel>(descriptor =>
            {
                var navArgs = new ConfigureCrawlerPageNavArgs
                {
                    ConfigureNew = false,
                    DescriptorToEdit = descriptor.BackingModel
                };
                NavigateConfigureDescriptor(navArgs);
            });

        private void NavigateConfigureDescriptor(ConfigureCrawlerPageNavArgs navArgs)
        {
            switch (navArgs.Domain)
            {
                case CrawlerDomain.Surugaya:
                    _navigationManager.Navigate(PageIndex.ConfigureSurugaya, navArgs);
                    break;
                case CrawlerDomain.Mandarake:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
