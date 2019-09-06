using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Utilities.Shared;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Sites.Lashinbang;
using AoTracker.Crawlers.Sites.Mercari;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Messaging;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.Util;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using AoTracker.Interfaces.Adapters;
using AoTracker.Resources;
using Autofac;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels
{
    public class CrawlerSetDetailsViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userDataProvider;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ISnackbarProvider _snackbarProvider;
        private readonly INavigationManager<PageIndex> _navigationManager;

        public override PageIndex PageIdentifier { get; } = PageIndex.CrawlerSetDetails;

        private bool _isAddingNew;
        private string _setName;
        private CrawlerSet _currentSet;

        private readonly List<CrawlerEntry> _crawlerEntries = new List<CrawlerEntry>
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
            new CrawlerEntry
            {

                CrawlerDomain = CrawlerDomain.Mercari,
                Title = "Mercari"
            },
            new CrawlerEntry
            {

                CrawlerDomain = CrawlerDomain.Yahoo,
                Title = "Yahoo"
            },
            new CrawlerEntry
            {

                CrawlerDomain = CrawlerDomain.Lashinbang,
                Title = "Lashinbang"
            },
        };

        private string _setNameError;

        public CrawlerSetDetailsViewModel(
            IUserDataProvider userDataProvider,
            ILifetimeScope lifetimeScope,
            ISnackbarProvider snackbarProvider,
            INavigationManager<PageIndex> navigationManager)
        {
            _userDataProvider = userDataProvider;
            _lifetimeScope = lifetimeScope;
            _snackbarProvider = snackbarProvider;
            _navigationManager = navigationManager;
            CrawlerEntries = _crawlerEntries
                .Select(entry => lifetimeScope.TypedResolve<CrawlerEntryViewModel>(entry, this))
                .ToList();

            MessengerInstance.Register<ConfigureCrawlerResultMessage>(this, OnConfigureCrawlerResult);
        }

        private async void OnToolbarAction(ToolbarActionMessage action)
        {
            if (action == ToolbarActionMessage.ClickedSaveButton)
            {
                ValidateSetName(SetName);

                if (SetNameError != null)
                {
                    return;
                }

                if (IsAddingNew)
                {
                    var set = new CrawlerSet
                    {
                        Guid = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow,
                        Descriptors = CrawlerDescriptors.Select(model => model.BackingModel).ToList(),
                        Name = SetName
                    };
                    await _userDataProvider.AddNewSet(set);
                }
                else
                {
                    _currentSet.Name = SetName;
                    _currentSet.Descriptors = CrawlerDescriptors.Select(model => model.BackingModel).ToList();
                    await _userDataProvider.UpdateSet(_currentSet);
                    MessengerInstance.Send(new CrawlerSetModifiedMessage(_currentSet));
                }
                _navigationManager.GoBack();
            }
        }

        private void OnConfigureCrawlerResult(ConfigureCrawlerResultMessage message)
        {
            //navigating back from configure crawler page
            if (message != null)
            {
                if (message.Action == ConfigureCrawlerResultMessage.ActionType.Add)
                {

                    var vm = _lifetimeScope.TypedResolve<CrawlerDescriptorViewModel>(
                        CrawlerDomainToCrawlerViewModelType(message.CrawlerDescriptor.CrawlerDomain),
                        new CrawlerDescriptor
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

        #region Lifecycle

        public void NavigatedTo(CrawlerSetDetailsPageNavArgs navArgs)
        {
            StartListeningForToolbarActions();

            var crawlerSet = navArgs.CrawlerSet;

            if (crawlerSet == _currentSet && crawlerSet != null && _currentSet != null)
            {
                CrawlerDescriptors.Clear();
                CrawlerDescriptors.AddRange(crawlerSet.Descriptors.Select(
                    descriptor =>
                    {
                        var type = CrawlerDomainToCrawlerViewModelType(descriptor.CrawlerDomain);
                        return _lifetimeScope.TypedResolve<CrawlerDescriptorViewModel>(type, descriptor);
                    }));
                return;
            }

            if (crawlerSet == null && navArgs.AddingNew)
            {
                _currentSet = null;
                PageTitle = _currentSet == null
                    ? AppResources.PageTitle_SetDetails_AddNew
                    : string.Format(AppResources.PageTitle_SetDetails, _currentSet.Name);


                IsAddingNew = true;
                SetName = string.Empty;
                CrawlerDescriptors.Clear();
            }
            else
            {
                _currentSet = crawlerSet;
                PageTitle = string.Format(AppResources.PageTitle_SetDetails, crawlerSet.Name);
                SetName = crawlerSet.Name;
                CrawlerDescriptors.AddRange(crawlerSet.Descriptors.Select(
                    descriptor =>
                    {
                        var type = CrawlerDomainToCrawlerViewModelType(descriptor.CrawlerDomain);
                        return _lifetimeScope.TypedResolve<CrawlerDescriptorViewModel>(type, descriptor);
                    }));
                IsAddingNew = false;
            }
        }

        public void NavigatedBack()
        {
            StartListeningForToolbarActions();
        }

        public void NavigatedFrom()
        {
            StopListeningForToolbarActions();
        }

        #endregion

        private Type CrawlerDomainToCrawlerViewModelType(CrawlerDomain crawlerDomain)
        {
            switch (crawlerDomain)
            {
                case CrawlerDomain.Surugaya:
                    return typeof(CrawlerDescriptorViewModel<SurugayaItem>);
                case CrawlerDomain.Mandarake:
                    return typeof(CrawlerDescriptorViewModel<MandarakeItem>);
                case CrawlerDomain.Yahoo:
                    return typeof(CrawlerDescriptorViewModel<YahooItem>);
                case CrawlerDomain.Mercari:
                    return typeof(CrawlerDescriptorViewModel<MercariItem>);
                case CrawlerDomain.Lashinbang:
                    return typeof(CrawlerDescriptorViewModel<LashinbangItem>);
                default:
                    throw new ArgumentOutOfRangeException(nameof(crawlerDomain), crawlerDomain, null);
            }

            throw new ArgumentOutOfRangeException(nameof(crawlerDomain));
        }

        private void StopListeningForToolbarActions()
        {
            MessengerInstance.Send(ToolbarRequestMessage.ResetToolbar);
            MessengerInstance.Unregister<ToolbarActionMessage>(this, OnToolbarAction);
        }

        private void StartListeningForToolbarActions()
        {
            MessengerInstance.Send(ToolbarRequestMessage.ShowSaveButton);
            MessengerInstance.Register<ToolbarActionMessage>(this, OnToolbarAction);
        }

        public List<CrawlerEntryViewModel> CrawlerEntries { get; }

        public SmartObservableCollection<CrawlerDescriptorViewModel> CrawlerDescriptors { get; } =
            new SmartObservableCollection<CrawlerDescriptorViewModel>();

        public string SetName
        {
            get => _setName;
            set
            {
                Set(ref _setName, value);
                if(SetNameError != null)
                    ValidateSetName(value);
            }
        }

        private void ValidateSetName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                SetNameError = AppResources.CrawlerSetDetails_Error_Title;
            }
            else
            {
                SetNameError = null;
            }
        }

        public bool IsAddingNew
        {
            get => _isAddingNew;
            set => Set(ref _isAddingNew, value);
        }

        public string SetNameError
        {
            get => _setNameError;
            set => Set(ref _setNameError, value);
        }

        public RelayCommand<CrawlerDescriptorViewModel> RemoveDescriptorCommand => new RelayCommand<CrawlerDescriptorViewModel>(
            descriptor =>
            {
                CrawlerDescriptors?.Remove(descriptor);
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
                    DescriptorToEdit = descriptor.BackingModel,
                    Domain = descriptor.BackingModel.CrawlerDomain
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
                    _navigationManager.Navigate(PageIndex.ConfigureMandarake, navArgs);
                    break;
                case CrawlerDomain.Yahoo:
                    _navigationManager.Navigate(PageIndex.ConfigureYahoo, navArgs);
                    break;
                case CrawlerDomain.Mercari:
                    _navigationManager.Navigate(PageIndex.ConfigureMercari, navArgs);
                    break;
                case CrawlerDomain.Lashinbang:
                    _navigationManager.Navigate(PageIndex.ConfigureLashinbang, navArgs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
