using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Infrastructure.ViewModels;
using Autofac;

namespace AoTracker.Infrastructure.Statics
{
    public static class ViewModelLocator
    {
        public static void RegisterViewModels(this ContainerBuilder builder)
        {
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<WelcomeViewModel>().SingleInstance();
            builder.RegisterType<AddSurugayaCrawlerViewModel>().SingleInstance();
            builder.RegisterType<CrawlerResultViewModel>().SingleInstance();
            builder.RegisterType<FeedViewModel>().SingleInstance();
            builder.RegisterType<CrawlerSetsViewModel>().SingleInstance();
        }
    }
}
