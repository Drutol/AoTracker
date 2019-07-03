using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Enums;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.Util;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using AoTracker.Resources;
using Autofac;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace AoTracker.Infrastructure.ViewModels
{
    public class CrawlerSetDetailsViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userDataProvider;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly INavigationManager _navigationManager;

        private bool _isAddingNew;
        private string _setName;
        private CrawlerSet _currentSet;
        private ConfigureCrawlerPageNavArgs _configureCrawlerNavArgs;
        private ObservableCollection<CrawlerDescriptorViewModel> _crawlerDescriptors;

        private List<CrawlerEntry> _crawlerEntries = new List<CrawlerEntry>
        {
            new CrawlerEntry
            {
                CrawlerDomain = CrawlerDomain.Surugaya,
                ImageSource = ImageSource.FromFile("surugaya.png"),
                Title = "Suruga-ya"
            },
            new CrawlerEntry
            {

                CrawlerDomain = CrawlerDomain.Mandarake,
                ImageSource = ImageSource.FromFile("mandarake.png"),
                Title = "Mandarake"
            },
        };



        public CrawlerSetDetailsViewModel(
            IUserDataProvider userDataProvider,
            ILifetimeScope lifetimeScope, 
            INavigationManager navigationManager)
        {
            _userDataProvider = userDataProvider;
            _lifetimeScope = lifetimeScope;
            _navigationManager = navigationManager;
            CrawlerEntries = _crawlerEntries
                .Select(entry => lifetimeScope.TypedResolve<CrawlerEntryViewModel>(entry, this))
                .ToList();
        }

        public void NavigatedTo(CrawlerSet crawlerSet)
        {
            if (crawlerSet == _currentSet)
                crawlerSet = null;
            

            if (crawlerSet == null)
            {
                Title = _currentSet == null
                    ? AppResources.PageTitle_SetDetails_AddNew
                    : string.Format(AppResources.PageTitle_SetDetails, _currentSet.Name);

                //navigating back from adding crawler
                if (_configureCrawlerNavArgs != null)
                {
                    if (_configureCrawlerNavArgs.Saved)
                    {
                        if (_configureCrawlerNavArgs.ConfigureNew)
                        {
                            var vm = _lifetimeScope.TypedResolve<CrawlerDescriptorViewModel>(new CrawlerDescriptor
                            {
                                CrawlerDomain = _configureCrawlerNavArgs.Domain,
                                CrawlerSourceParameters = _configureCrawlerNavArgs.CrawlerSourceParameters
                            });
                            CrawlerDescriptors.Add(vm);
                        }
                        else
                        {
                            CrawlerDescriptors
                                .First(model => model.BackingModel == _configureCrawlerNavArgs.DescriptorToEdit)
                                .CrawlerSourceParameters = _configureCrawlerNavArgs.CrawlerSourceParameters;
                        }
                    }

                    _configureCrawlerNavArgs = null;
                }
                else
                {
                    CrawlerDescriptors = new ObservableCollection<CrawlerDescriptorViewModel>();
                    IsAddingNew = true;
                }
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
            set => Set(ref _setName, value, () =>
            {
                RaisePropertyChanged(() => CanSave);
            });
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

        public RelayCommand<CrawlerEntry> AddCrawlerCommand => new RelayCommand<CrawlerEntry>(entry =>
        {
            _configureCrawlerNavArgs = new ConfigureCrawlerPageNavArgs
            {
                ConfigureNew = true,
                Domain = entry.CrawlerDomain
            };
            NavigateConfigureDescriptor();
        });

        public RelayCommand<CrawlerDescriptor> SelectCrawlerDescriptorCommand =>
            new RelayCommand<CrawlerDescriptor>(descriptor =>
            {
                _configureCrawlerNavArgs = new ConfigureCrawlerPageNavArgs
                {
                    ConfigureNew = false,
                    DescriptorToEdit = descriptor
                };
                NavigateConfigureDescriptor();
            });

        private void NavigateConfigureDescriptor()
        {
            switch (_configureCrawlerNavArgs.Domain)
            {
                case CrawlerDomain.Surugaya:
                    _navigationManager.PushPage(PageIndex.ConfigureSurugaya, _configureCrawlerNavArgs);
                    break;
                case CrawlerDomain.Mandarake:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
