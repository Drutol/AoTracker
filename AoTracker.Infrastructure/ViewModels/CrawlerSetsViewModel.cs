using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels
{
    public class CrawlerSetsViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userDataProvider;
        private readonly INavigationManager<PageIndex> _navigationManager;

        private ObservableCollection<CrawlerSet> _sets;
        private CrawlerSet _selectedSet;

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

        private void OnSetSelected(CrawlerSet set)
        {
            _navigationManager.Navigate(PageIndex.CrawlerSetDetails, set);
        }

        public ObservableCollection<CrawlerSet> Sets
        {
            get => _sets;
            set => Set(ref _sets, value);
        }

        public CrawlerSet SelectedSet
        {
            get => _selectedSet;
            set => Set(ref _selectedSet, value, OnSetSelected);
        }

        public RelayCommand AddNewSetCommand => new RelayCommand(() =>
        {
            _navigationManager.Navigate(PageIndex.CrawlerSetDetails);
        });
    }
}
