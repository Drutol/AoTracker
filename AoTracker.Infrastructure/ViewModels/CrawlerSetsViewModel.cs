using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.ViewModels
{
    public class CrawlerSetsViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userDataProvider;
        private readonly INavigationManager _navigationManager;

        private ObservableCollection<CrawlerSet> _sets;
        private CrawlerSet _selectedSet;

        public CrawlerSetsViewModel(
            IUserDataProvider userDataProvider,
            INavigationManager navigationManager)
        {
            _userDataProvider = userDataProvider;
            _navigationManager = navigationManager;
        }

        public void NavigatedTo()
        {
            Sets = new ObservableCollection<CrawlerSet>(_sets);
        }

        private void OnSetSelected()
        {
            _navigationManager.PushPage(PageIndex.CrawlerSetDetails, SelectedSet);
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
    }
}
