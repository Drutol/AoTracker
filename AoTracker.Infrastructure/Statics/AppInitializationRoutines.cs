using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Interfaces;
using Autofac;
using Microsoft.AppCenter.Crashes;

namespace AoTracker.Infrastructure.Statics
{
    public static class AppInitializationRoutines
    {
        private static IContainer Container { get; set; }

        public static void InitializeDependencyInjection(
            Action<ContainerBuilder> dependenciesRegistration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterViewModels();
            builder.RegisterResources();
            dependenciesRegistration(builder);

            Container = builder.Build();

            Crashes.GetErrorAttachments = report =>
            {
                var provider = Container.Resolve<ICrashDumpLogProvider>();
                return new[]
                {
                    ErrorAttachmentLog.AttachmentWithText(provider.GetLogs(), "logs.txt"),
                };
            };
        }

        public static async void InitializeDependencies()
        {
            var initializables = Container.Resolve<IEnumerable<IInitializable>>()
                .Select(initializable => initializable.Initialize()).ToList();
            await Task.WhenAll(initializables);
        }

        public static void InitializeDependenciesForBackground(Action<ContainerBuilder> dependenciesRegistration)
        {
            if (Container != null)
                return;

            var builder = new ContainerBuilder();

            builder.RegisterResources();
            dependenciesRegistration(builder);

            Container = builder.Build();
        }
    }
}
