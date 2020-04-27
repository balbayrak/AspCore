using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AspCore.Dependency.DependencyAttributes;

namespace AspCore.Dependency.Concrete
{
    public static class TypeMapHelper
    {
        public static IEnumerable<TypeMap> GetTypeMaps<TInterface>(Assembly[] assemblies, string namespaceStr = null, bool getExposed = false)
        {
            List<Assembly> assembliesList = assemblies.ToList();
            IEnumerable<Type> types = assembliesList.SelectMany(asm => asm.DefinedTypes).Select(x => x.AsType());

            if (string.IsNullOrEmpty(namespaceStr?.Trim()))
            {
                types = types.Where(t => t.IsNonAbstractClass(true) && typeof(TInterface).IsAssignableFrom(t));
            }
            else
            {
                types = types.Where(t => t.IsNonAbstractClass(true) && t.Namespace.StartsWith(namespaceStr) && typeof(TInterface).IsAssignableFrom(t));
            }

            Func<TypeInfo, IEnumerable<Type>> selector = t => t.ImplementedInterfaces
                .Where(x => x.HasMatchingGenericParameterCount(t))
                .Select(x => x.GetRegistrationType(t));

            Func<Type, IEnumerable<Type>> selector1 = t => selector(t.GetTypeInfo());
            if (getExposed)
            {
                IEnumerable<TypeMap> maps = types.Select(t => new TypeMap(t, t.GetTypeInfo().GetInterfaces(), ExposedServiceHelper.GetExposedServices(t)));

                var typeMaps = maps.ToList();

                List<Type> exposedTypeList = typeMaps.Where(t => t.ExposedTypes.Any()).SelectMany(typeMap => typeMap.ExposedTypes).ToList();
                var notExistsExposedType = typeMaps.Where(t => !t.ExposedTypes.Any());
                List<TypeMap> deletedTypeMaps = notExistsExposedType.Where(typeMap => typeMap.ServiceTypes.Intersect(exposedTypeList).Any()).ToList();
                if (deletedTypeMaps.Any())
                {
                    typeMaps.RemoveAll(t => deletedTypeMaps.Contains(t));
                }
                return typeMaps;
            }
            else
            {
                IEnumerable<TypeMap> maps = types.Select(t => new TypeMap(t, t.GetTypeInfo().GetInterfaces(), null));
                return maps;
            }

        }
    }
}
