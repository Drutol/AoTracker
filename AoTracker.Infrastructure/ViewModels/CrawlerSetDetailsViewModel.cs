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

namespace AoTracker.Infrastructure.ViewModels
{
    public class CrawlerSetDetailsViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userDataProvider;
        private readonly INavigationManager _navigationManager;

        private CrawlerSet _currentSet;
        private bool _isAddingNew;
        private string _setName;
        private ConfigureCrawlerPageNavArgs _configureCrawlerNavArgs;
        private ObservableCollection<CrawlerDescriptor> _crawlerDescriptors;

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

        public CrawlerSetDetailsViewModel(IUserDataProvider userDataProvider,
            ILifetimeScope lifetimeScope, INavigationManager navigationManager)
        {
            _userDataProvider = userDataProvider;
            _navigationManager = navigationManager;
            CrawlerViewModelEntries = _crawlerEntries
                .Select(entry => lifetimeScope.TypedResolve<CrawlerEntryViewModel>(entry, this))
                .ToList();
        }

        public void NavigatedTo(CrawlerSet crawlerSet)
        {


            if (crawlerSet == null)
            {
                Title = AppResources.PageTitle_SetDetails_AddNew;
                //navigating back from adding crawler
                if (_configureCrawlerNavArgs != null)
                {
                    CrawlerDescriptors.Add(new CrawlerDescriptor
                    {
                        CrawlerDomain = _configureCrawlerNavArgs.Domain,
                        CrawlerSourceParameters = _configureCrawlerNavArgs.CrawlerSourceParameters
                    });
                }
                else
                {
                    CrawlerDescriptors = new ObservableCollection<CrawlerDescriptor>();
                    IsAddingNew = true;
                }
            }
            else
            {
                Title = string.Format(AppResources.PageTitle_SetDetails, crawlerSet.Name);
                SetName = crawlerSet.Name;
                CrawlerDescriptors = new ObservableCollection<CrawlerDescriptor>(crawlerSet.Descriptors);
                IsAddingNew = false;
            }
        }

        public List<CrawlerEntryViewModel> CrawlerViewModelEntries { get; }

        public ObservableCollection<CrawlerDescriptor> CrawlerDescriptors
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

        public RelayCommand<CrawlerEntry> AddCrawlerCommand => new RelayCommand<CrawlerEntry>(entry =>
        {
            _configureCrawlerNavArgs = new ConfigureCrawlerPageNavArgs();
            switch (entry.CrawlerDomain)
            {
                case CrawlerDomain.Surugaya:
                    _navigationManager.PushPage(PageIndex.ConfigureSurugaya, _configureCrawlerNavArgs);
                    break;
                case CrawlerDomain.Mandarake:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        });
    }
}
