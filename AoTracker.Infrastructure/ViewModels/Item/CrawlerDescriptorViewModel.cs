using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Item
{
    public class CrawlerDescriptorViewModel : ItemViewModelBase<CrawlerDescriptor>
    {
        private readonly IVersionProvider _versionProvider;
        private readonly CrawlerSetDetailsViewModel _parent;

        public CrawlerDescriptorViewModel(IVersionProvider versionProvider, CrawlerDescriptor item, CrawlerSetDetailsViewModel parent) : base(item)
        {
            _versionProvider = versionProvider;
            _parent = parent;
        }

        public ICrawlerSourceParameters CrawlerSourceParameters
        {
            get => BackingModel.CrawlerSourceParameters;
            set
            {
                BackingModel.CrawlerSourceParameters = value;
                RaisePropertyChanged();
            }
        }

        public string FormattedPriceIncrease => $"+{BackingModel.CrawlerSourceParameters.OffsetIncrease}¥ " +
                                                $"+{BackingModel.CrawlerSourceParameters.PercentageIncrease}%";

        public string FormattedIgnoredItems => string.Join(", ", BackingModel.CrawlerSourceParameters.ExcludedKeywords);

        public bool AreAnyExcludedKeywordsPresent => BackingModel.CrawlerSourceParameters.ExcludedKeywords?.Any() ?? false;

        public bool AreAnyPriceIncreasesPresent => Math.Abs(BackingModel.CrawlerSourceParameters.OffsetIncrease) > 0.01 ||
                                                   Math.Abs(BackingModel.CrawlerSourceParameters.PercentageIncrease) > 0.01;
    }

    public class CrawlerDescriptorViewModel<T> : CrawlerDescriptorViewModel where T : ICrawlerResultItem
    {
        public CrawlerDescriptorViewModel(IVersionProvider versionProvider, CrawlerDescriptor item,
            CrawlerSetDetailsViewModel parent) : base(versionProvider, item, parent)
        {
        }
    }
}
