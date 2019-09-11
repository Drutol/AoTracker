using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Utilities.Shared;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
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
    public class CrawlerSetsViewModel : ViewModelBase
    {
        private readonly IVersionProvider _versionProvider;
        private readonly IUserDataProvider _userDataProvider;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly INavigationManager<PageIndex> _navigationManager;

        public override PageIndex PageIdentifier { get; } = PageIndex.CrawlerSets;

        public CrawlerSetsViewModel(
            IVersionProvider versionProvider,
            IUserDataProvider userDataProvider,
            ILifetimeScope lifetimeScope,
            INavigationManager<PageIndex> navigationManager)
        {
            _versionProvider = versionProvider;
            _userDataProvider = userDataProvider;
            _lifetimeScope = lifetimeScope;
            _navigationManager = navigationManager;

            PageTitle = AppResources.PageTitle_CrawlerSets;
        }

        public void NavigatedTo()
        {
            Sets.Clear();
            var items = _userDataProvider.CrawlingSets.Select(set =>
                _lifetimeScope.TypedResolve<CrawlerSetViewModel>(set));
            Sets.PlatformAddRange(items, _versionProvider.Platform);

        }

        public SmartObservableCollection<CrawlerSetViewModel> Sets { get; } =
            new SmartObservableCollection<CrawlerSetViewModel>();


        public RelayCommand AddNewSetCommand => new RelayCommand(() =>
        {
            _navigationManager.Navigate(PageIndex.CrawlerSetDetails, CrawlerSetDetailsPageNavArgs.AddNew);
        });

        public RelayCommand<CrawlerSetViewModel> NavigateSetCommand => new RelayCommand<CrawlerSetViewModel>(set =>
        {
            _navigationManager.Navigate(PageIndex.CrawlerSetDetails,
                new CrawlerSetDetailsPageNavArgs(set.BackingModel));
        });

        public void MoveCrawlerSet(int movedPosition, int targetPosition)
        {
            Sets.Move(movedPosition, targetPosition);
            _userDataProvider.MoveSet(movedPosition, targetPosition);
        }

        public void RemoveSet(CrawlerSetViewModel set)
        {
            Sets.Remove(set);
            _userDataProvider.RemoveSet(set.BackingModel);
        }


    }
}
