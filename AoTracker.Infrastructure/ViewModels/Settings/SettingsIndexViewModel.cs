using System.Collections.Generic;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Settings
{
    public class SettingsIndexViewModel : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;

        public SettingsIndexViewModel(INavigationManager<PageIndex> navigationManager)
        {
            _navigationManager = navigationManager;
            PageTitle = AppResources.PageTitle_SettingsIndex;
        }

        public List<SettingsIndexEntry> Entries { get; } = new List<SettingsIndexEntry>
        {
            new SettingsIndexEntry
            {
                Page = PageIndex.SettingsGeneral,
                Title = AppResources.SettingsIndex_Entry_General_Title,
                Subtitle = AppResources.SettingsIndex_Entry_General_Subtitle
            },
            new SettingsIndexEntry
            {
                Page = PageIndex.SettingsAbout,
                Title = AppResources.SettingsIndex_Entry_About_Title,
                Subtitle = AppResources.SettingsIndex_Entry_About_Subtitle
            }
        };

        public RelayCommand<SettingsIndexEntry> SelectEntryCommand =>
            new RelayCommand<SettingsIndexEntry>(entry =>
            {
                _navigationManager.Navigate(entry.Page);
            });
    }
}
