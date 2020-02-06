using System;
using System.Collections;
using System.Reflection;

namespace AspCore.Utilities.Mapper.Concrete
{
    public class GenericSetter<TSource, TDestination> : SetterBase<TSource, TDestination>
        where TDestination : class, new()
        where TSource : class, new()
    {
        public override void SetValue(TSource source, TDestination destination, PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            var value = sourceProperty.GetValue(source);
            var list = (IList)Activator.CreateInstance(destinationProperty.PropertyType);
            foreach (var ins in (IEnumerable)value)
            {
                var instance = Activator.CreateInstance(list.GetType().GetGenericArguments()[0]);
                instance = SetProperties(ins, instance);
                list.Add(instance);
            }
            destinationProperty.SetValue(destination, list, null);

        }
    }
}
