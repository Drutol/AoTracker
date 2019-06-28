using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace AoTracker.Infrastructure.Statics
{
    public class AppInitializationRoutines
    {
        private static IContainer Container { get; set; }

        public static Func<Type, object> InitializeDependencyInjection()
        {
            var builder = new ContainerBuilder();

            builder.RegisterViewModels();
            builder.RegisterResources();

            Container = builder.Build();
        }
    }
}
