using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.ViewModels.Item
{
    public class CrawlerSetViewModel : ItemViewModelBase<CrawlerSet>
    {
        private readonly IUserDataProvider _userDataProvider;

        public CrawlerSetViewModel(CrawlerSet item, IUserDataProvider userDataProvider) : base(item)
        {
            _userDataProvider = userDataProvider;
        }

        public bool IsFavourite
        {
            get => BackingModel.IsFavourite;
            set
            {
                BackingModel.IsFavourite = value;
                _userDataProvider.UpdateSet(BackingModel);
                MessengerInstance.Send(new CrawlerSetModifiedMessage(true));
                RaisePropertyChanged();
            }
        }
    }
}
