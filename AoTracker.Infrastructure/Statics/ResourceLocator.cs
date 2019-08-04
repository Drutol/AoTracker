using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Infrastructure.Crawling;
using AoTracker.Infrastructure.Infrastructure;
using AoTracker.Interfaces;
using Autofac;

namespace AoTracker.Infrastructure.Statics
{
    public static class ResourceLocator
    {
        private static ILifetimeScope _lifetime;

        public static void RegisterResources(this ContainerBuilder builder)
        {
            builder.RegisterType<Settings>().As<ISettings>().SingleInstance();
            builder.RegisterType<CrawlerManagerProvider>().As<ICrawlerManagerProvider>().SingleInstance();
            builder.RegisterType<FeedProvider>().As<IFeedProvider>().SingleInstance();
            builder.RegisterType<UserDataProvider>().As<IUserDataProvider>().SingleInstance();
            builder.RegisterType<DataCache>().As<IDataCache>().SingleInstance();
            builder.RegisterType<AppVariables>().SingleInstance();

            builder.RegisterBuildCallback(BuildCallback);

        }

        private static void BuildCallback(IContainer obj)
        {
            _lifetime = obj.BeginLifetimeScope();
        }

        public static ILifetimeScope ObtainScope() => _lifetime.BeginLifetimeScope();

    }
}
