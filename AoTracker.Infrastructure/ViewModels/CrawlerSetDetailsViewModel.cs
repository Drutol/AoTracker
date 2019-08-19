﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AoLibs.Navigation.Core.Interfaces;
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

        private async void OnToolbarAction(ToolbarActionMessage action)
        {
            if (action == ToolbarActionMessage.ClickedSaveButton)
            {
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
                if(_currentSet != null)
                    _currentSet.Descriptors = CrawlerDescriptors.Select(model => model.BackingModel).ToList();
            }
        }

        #region Lifecycle

        public void NavigatedTo(CrawlerSetDetailsPageNavArgs navArgs)
        {
            StartListeningForToolbarActions();

            var crawlerSet = navArgs?.CrawlerSet;

            if (crawlerSet == _currentSet && crawlerSet != null)
                return;

            if (crawlerSet == null)
            {
                Title = _currentSet == null
                    ? AppResources.PageTitle_SetDetails_AddNew
                    : string.Format(AppResources.PageTitle_SetDetails, _currentSet.Name);

                CrawlerDescriptors = new ObservableCollection<CrawlerDescriptorViewModel>();
                IsAddingNew = true;

                SetName = string.Empty;
                CrawlerDescriptors = new ObservableCollection<CrawlerDescriptorViewModel>();
            }
            else
            {
                _currentSet = crawlerSet;
                Title = string.Format(AppResources.PageTitle_SetDetails, crawlerSet.Name);
                SetName = crawlerSet.Name;
                CrawlerDescriptors = new ObservableCollection<CrawlerDescriptorViewModel>(crawlerSet.Descriptors.Select(
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

        public ObservableCollection<CrawlerDescriptorViewModel> CrawlerDescriptors
        {
            get => _crawlerDescriptors;
            set => Set(ref _crawlerDescriptors, value);
        }

        public string SetName
        {
            get => _setName;
            set
            {
                Set(ref _setName, value, _ => { RaisePropertyChanged(() => CanSave); });
                RaisePropertyChanged();
            }
        }

        public bool IsAddingNew
        {
            get => _isAddingNew;
            set
            {
                _isAddingNew = value;
                RaisePropertyChanged();
            }
        }

        public bool CanSave => true;

        public RelayCommand<CrawlerDescriptorViewModel> RemoveDescriptorCommand => new RelayCommand<CrawlerDescriptorViewModel>(
            descriptor =>
            {
                _crawlerDescriptors.Remove(descriptor);
                if(_currentSet.Descriptors.Contains(descriptor.BackingModel))
                    _currentSet.Descriptors.Remove(descriptor.BackingModel);
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
