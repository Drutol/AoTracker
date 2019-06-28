using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace AoTracker.Infrastructure.Statics
{
    public class AppInitializationRoutines
    {
        private static IContainer Container { get; set; }

        public static Func<Type, object> InitializeDependencyInjection(
            Action<ContainerBuilder> dependenciesRegistration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterViewModels();
            builder.RegisterResources();
            dependenciesRegistration(builder);

            Container = builder.Build();

            return type => Container.ResolveOptional(type);
        }
    }
}
