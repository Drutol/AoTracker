using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Domain.Models;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace AoTracker.Infrastructure.ViewModels
{
    public class CrawlerSetDetailsViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userDataProvider;
        private CrawlerSet _currentSet;
        private bool _isAddingNew;
        private string _nameInput;

        public List<CrawlerEntry> CrawlerEntries { get; } = new List<CrawlerEntry>
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

        public CrawlerSetDetailsViewModel(IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        public void NavigatedTo(CrawlerSet crawlerSet)
        {
            if (crawlerSet == null)
            {
                IsAddingNew = true;
                CurrentSet = new CrawlerSet();
            }
            else
            {
                CurrentSet = crawlerSet;
                IsAddingNew = false;
            }
        }

        public string NameInput
        {
            get => _nameInput;
            set => Set(ref _nameInput, value, () =>
            {
                CurrentSet.Name = NameInput;
                RaisePropertyChanged(nameof(CanSave));
            });
        }

        public CrawlerSet CurrentSet
        {
            get => _currentSet;
            set => Set(ref _currentSet, value);
        }

        public bool IsAddingNew
        {
            get => _isAddingNew;
            set => Set(ref _isAddingNew, value);
        }

        public bool CanSave => CurrentSet.IsValid;

        public RelayCommand<CrawlerEntry> AddCrawlerCommand => new RelayCommand<CrawlerEntry>(entry =>
        {

        });

        public class CrawlerEntry
        {
            public CrawlerDomain CrawlerDomain { get; set; }
            public string Title { get; set; }
            public ImageSource ImageSource { get; set; }
        }
    }
}
