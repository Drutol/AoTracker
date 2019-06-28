using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Infrastructure.Crawling;
using AoTracker.Infrastructure.Infrastructure;
using AoTracker.Interfaces;
using Autofac;

namespace AoTracker.Infrastructure.Statics
{
    public static class ResourceLocator
    {
        public static void RegisterResources(this ContainerBuilder builder)
        {
            builder.RegisterType<Settings>().As<ISettings>().SingleInstance();
            builder.RegisterType<CrawlerManagerProvider>().As<ICrawlerManagerProvider>().SingleInstance();

        }
    }
}
