using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AspCore.Dependency.Concrete
{
    public static class TypeMapHelper
    {
        public static IEnumerable<TypeMap> GetTypeMaps<TInterface>(Assembly[] assemblies, string namespaceStr = null)
        {
            List<Assembly> assembliesList = assemblies.ToList();
            IEnumerable<Type> types = assembliesList.SelectMany(asm => asm.DefinedTypes).Select(x => x.AsType());

            if (string.IsNullOrEmpty(namespaceStr?.Trim()))
            {
                var type = types.Where(t => typeof(TInterface).IsAssignableFrom(t)).ToList();
                types = types.Where(t => t.IsNonAbstractClass(true) && typeof(TInterface).IsAssignableFrom(t));
            }
            else
            {
                types = types.Where(t => t.IsNonAbstractClass(true) && t.Namespace.StartsWith(namespaceStr) && typeof(TInterface).IsAssignableFrom(t));
            }

            Func<TypeInfo, IEnumerable<Type>> selector = t => t.ImplementedInterfaces
                .Where(x => x.HasMatchingGenericArity(t))
                .Select(x => x.GetRegistrationType(t));

            Func<Type, IEnumerable<Type>> selector1 = t => selector(t.GetTypeInfo());
            IEnumerable<TypeMap> maps = types.Select(t => new TypeMap(t, selector1(t)));

            return maps;
        }
    }
}
