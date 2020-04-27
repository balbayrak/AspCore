using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Dependency.Concrete
{
    public struct TypeMap
    {
        public TypeMap(Type implementationType, IEnumerable<Type> serviceTypes, IEnumerable<Type> exposedTypes)
        {
            ImplementationType = implementationType;
            ServiceTypes = serviceTypes;
            ExposedTypes = exposedTypes;
        }

        public Type ImplementationType { get; }

        public IEnumerable<Type> ServiceTypes { get; }

        public IEnumerable<Type> ExposedTypes { get; }
    }

    public class TypeDto
    {
        public Type Type { get; set; }
        public List<Type> TypeList { get; set; }
    }
}
