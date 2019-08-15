using System;
using System.Linq;
using Autofac;

namespace AoTracker.Infrastructure.Util
{
    public static class AutoFacExtensions
    {
        public static TReturn TypedResolve<TReturn, TParameter>(this ILifetimeScope scope, TParameter parameter)
        {
            return scope.Resolve<TReturn>(new TypedParameter(typeof(TParameter), parameter));
        }

        public static TReturn TypedResolve<TReturn>(this ILifetimeScope scope, params object[] parameter)
        {
            return scope.Resolve<TReturn>(parameter.Select(o => new TypedParameter(o.GetType(), o)));
        }

        public static TReturn TypedResolve<TReturn>(this ILifetimeScope scope, Type type, params object[] parameter)
        {
            return (TReturn)scope.Resolve(type, parameter.Select(o => new TypedParameter(o.GetType(), o)));
        }
    }
}
