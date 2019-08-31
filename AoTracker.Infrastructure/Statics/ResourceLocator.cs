using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Infrastructure.Crawling;
using AoTracker.Infrastructure.Infrastructure;
using AoTracker.Infrastructure.LinkHandlers;
using AoTracker.Infrastructure.Logging;
using AoTracker.Interfaces;
using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AoTracker.Infrastructure.Statics
{
    public static class ResourceLocator
    {
        private static ILifetimeScope _lifetime;

        public static void RegisterResources(this ContainerBuilder builder)
        {
            builder.RegisterType<Settings>().As<ISettings>().SingleInstance();
            builder.RegisterType<CrawlerManagerProvider>().As<ICrawlerManagerProvider>().SingleInstance();
            builder.RegisterType<UserDataProvider>().As<IUserDataProvider>().SingleInstance();
            builder.RegisterType<DataCache>().As<IDataCache>().SingleInstance();
            builder.RegisterType<FeedHistoryProvider>().As<IFeedHistoryProvider>().SingleInstance();
            builder.RegisterType<TelemetryProvider>().As<ITelemetryProvider>().SingleInstance();
            builder.RegisterType<IgnoredItemsManager>().As<IIgnoredItemsManager>().As<IInitializable>().SingleInstance();
            builder.RegisterType<WatchedItemsManager>().As<IWatchedItemsManager>().As<IInitializable>().SingleInstance();
            builder.RegisterType<ChangelogHandler>().As<IInitializable>().SingleInstance();

            builder.RegisterType<DomainLinkHandlerManager>().As<IDomainLinkHandlerManager>().SingleInstance();
            builder.RegisterType<DefaultDomainLinkHandler>().As<IDomainLinkHandler>().SingleInstance();
            builder.RegisterType<ZenMarketLinkHandler>().As<IDomainLinkHandler>().SingleInstance();

            builder.RegisterType<AppVariables>().SingleInstance();

            builder.RegisterType<FeedProvider>().As<IFeedProvider>();

            builder.RegisterType<AppCenterCrashDumpLoggerProvider>().As<ILoggerProvider>().As<ICrashDumpLogProvider>().SingleInstance();
            builder.Register(context => new LoggerFactory(context.Resolve<IEnumerable<ILoggerProvider>>())).As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
            
            builder.RegisterBuildCallback(BuildCallback);
        }

        public static void BeginNewLifetimeScope()
        {
            CurrentScope.Dispose();
            CurrentScope = _lifetime.BeginLifetimeScope();
        }

        public static ILifetimeScope ObtainScope() => _lifetime.BeginLifetimeScope();

        public static ILifetimeScope CurrentScope { get; private set; }

        private static void BuildCallback(IContainer obj)
        {
            _lifetime = obj.BeginLifetimeScope();
            CurrentScope = _lifetime.BeginLifetimeScope();
        }
    }
}
