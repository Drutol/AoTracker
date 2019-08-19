using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels
{
    public class CrawlerSetsViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userDataProvider;
        private readonly INavigationManager<PageIndex> _navigationManager;

        private ObservableCollection<CrawlerSet> _sets;

        public CrawlerSetsViewModel(
            IUserDataProvider userDataProvider,
            INavigationManager<PageIndex> navigationManager)
        {
            _userDataProvider = userDataProvider;
            _navigationManager = navigationManager;

            Title = "Crawler Sets";
        }

        public void NavigatedTo()
        {
            Sets = new ObservableCollection<CrawlerSet>(_userDataProvider.CrawlingSets);
        }

        public ObservableCollection<CrawlerSet> Sets
        {
            get => _sets;
            set => Set(ref _sets, value);
        }

        public RelayCommand AddNewSetCommand => new RelayCommand(() =>
        {
            _navigationManager.Navigate(PageIndex.CrawlerSetDetails);
        });

        public RelayCommand<CrawlerSet> NavigateSetCommand => new RelayCommand<CrawlerSet>(set =>
        {
            _navigationManager.Navigate(PageIndex.CrawlerSetDetails, new CrawlerSetDetailsPageNavArgs(set));
        });

        public void MoveCrawlerSet(int movedPosition, int targetPosition)
        {
            Sets.Move(movedPosition, targetPosition);
            _userDataProvider.MoveSet(movedPosition, targetPosition);
        }

        public void RemoveSet(CrawlerSet set)
        {
            Sets.Remove(set);
            _userDataProvider.RemoveSet(set);
        }
    }
}
