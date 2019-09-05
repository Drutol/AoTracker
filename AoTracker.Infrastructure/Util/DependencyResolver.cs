using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Infrastructure.Statics;
using Autofac;

namespace AoTracker.Infrastructure.Util
{
    public class DependencyResolver : IDependencyResolver, ICustomDialogDependencyResolver
    {
        public TDependency Resolve<TDependency>()
        {
            try
            {
                return ResourceLocator.CurrentScope.Resolve<TDependency>();
            }
            catch (Exception e)
            {
                Debugger.Break();
                throw;
            }
        }
    }
}
