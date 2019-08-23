using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Interfaces;
using Autofac;

namespace AoTracker.Infrastructure.Statics
{
    public class AppInitializationRoutines
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
        }

        public static async void InitializeDependencies()
        {
            var initializables = Container.Resolve<IEnumerable<IInitializable>>()
                .Select(initializable => initializable.Initialize()).ToList();
            await Task.WhenAll(initializables);
        }
    }
}
