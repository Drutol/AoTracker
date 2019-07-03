﻿using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Item
{
    public class CrawlerDescriptorViewModel : ItemViewModelBase<CrawlerDescriptor>
    {
        private readonly CrawlerSetDetailsViewModel _parent;

        public CrawlerDescriptorViewModel(CrawlerDescriptor item, CrawlerSetDetailsViewModel parent) : base(item)
        {
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

        public RelayCommand<CrawlerDescriptor> RemoveCommand => _parent.RemoveDescriptorCommand;

        public RelayCommand<CrawlerDescriptor> TapCommand => _parent.SelectCrawlerDescriptorCommand;

    }
}