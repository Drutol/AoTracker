﻿using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Crawlers;
using AoTracker.Infrastructure.ViewModels.Feed;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Infrastructure.ViewModels.Settings;
using Autofac;

namespace AoTracker.Infrastructure.Statics
{
    public static class ViewModelLocator
    {
        public static void RegisterViewModels(this ContainerBuilder builder)
        {
            // standard
            builder.RegisterType<MainViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<WelcomeViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<CrawlerResultViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<FeedViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<CrawlerSetsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<CrawlerSetDetailsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsIndexViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsIndexViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsAboutViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsGeneralViewModel>().InstancePerLifetimeScope();

            // one shots
            builder.RegisterType<ConfigureSurugayaCrawlerViewModel>();
            builder.RegisterType<ConfigureMandarakeCrawlerViewModel>();
            builder.RegisterType<ConfigureYahooCrawlerViewModel>();
            builder.RegisterType<ConfigureMercariCrawlerViewModel>();
            builder.RegisterType<ConfigureLashinbangCrawlerViewModel>();

            // items
            builder.RegisterType<CrawlerEntryViewModel>();
            builder.RegisterType<FeedItemViewModel>();
            builder.RegisterType<FeedTabViewModel>();
            builder.RegisterGeneric(typeof(CrawlerDescriptorViewModel<>));
            builder.RegisterGeneric(typeof(FeedItemViewModel<>));
        }
    }
}
