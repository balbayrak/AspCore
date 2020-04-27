using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace AspCore.Dependency.DependencyAttributes
{
    public class ExposedServiceAttribute: Attribute,IExposedServiceProvider
    {
        public Type[] ServiceTypes { get; }

        public bool IncludeDefaults { get; set; }

        public bool IncludeSelf { get; set; }

        public ExposedServiceAttribute(params Type[] serviceTypes)
        {
            ServiceTypes = serviceTypes ?? new Type[0];
        }
        public Type[] GetExposedServiceTypes(Type targetType)
        {
            var serviceList = ServiceTypes.ToList();

            if (IncludeDefaults)
            {
                foreach (var type in GetDefaultServices(targetType))
                {
                    AddIfNotContains(serviceList,type);
                }

                if (IncludeSelf)
                {
                    AddIfNotContains(serviceList,targetType);
                }
            }
            else if (IncludeSelf)
            {
                AddIfNotContains(serviceList, targetType);
            }

            return serviceList.ToArray();
        }

        private static List<Type> GetDefaultServices(Type type)
        {
            var serviceTypes = new List<Type>();

            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                var interfaceName = interfaceType.Name;

                if (interfaceName.StartsWith("I"))
                {
                    var len = interfaceName.Length - 1;
                    interfaceName = interfaceName.Substring(interfaceName.Length - len, len);
                }
                if (type.Name.Intersect(interfaceName).Count() > 4)
                {
                    serviceTypes.Add(interfaceType);
                }
            }

            return serviceTypes;
        }


        private void AddIfNotContains<T>([NotNull]ICollection<T> source, T item)
        {
            if (source.Contains(item))
            {
              return;
            }
            source.Add(item);
        }
    }
}
