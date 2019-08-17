using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Crawlers;
using AoTracker.Infrastructure.ViewModels.Feed;
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
