using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspCore.Dependency.DependencyAttributes
{
    public static class ExposedServiceHelper
    {
        private static readonly ExposedServiceAttribute DefaultExposedServiceAttribute =
            new ExposedServiceAttribute
            {
                IncludeDefaults = false,
                IncludeSelf = false
            };

        public static List<Type> GetExposedServices(Type type)
        {
            return type
                .GetCustomAttributes(true)
                .OfType<IExposedServiceProvider>()
                .DefaultIfEmpty(DefaultExposedServiceAttribute)
                .SelectMany(p => p.GetExposedServiceTypes(type))
                .Distinct()
                .ToList();
        }
    }
}
