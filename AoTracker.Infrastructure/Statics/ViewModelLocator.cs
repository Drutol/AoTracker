﻿using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;
using Autofac;

namespace AoTracker.Infrastructure.Statics
{
    public static class ViewModelLocator
    {
        public static void RegisterViewModels(this ContainerBuilder builder)
        {
            // standard
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<WelcomeViewModel>().SingleInstance();
            builder.RegisterType<CrawlerResultViewModel>().SingleInstance();
            builder.RegisterType<FeedViewModel>().SingleInstance();
            builder.RegisterType<CrawlerSetsViewModel>().SingleInstance();
            builder.RegisterType<CrawlerSetDetailsViewModel>().SingleInstance();

            // one shots
            builder.RegisterType<ConfigureSurugayaCrawlerViewModel>();

            // items
            builder.RegisterType<CrawlerEntryViewModel>();
            builder.RegisterType<CrawlerDescriptorViewModel>();
            builder.RegisterType<FeedItemViewModel>();
        }
    }
}
