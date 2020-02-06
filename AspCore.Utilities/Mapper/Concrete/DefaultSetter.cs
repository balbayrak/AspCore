using System;
using System.Reflection;

namespace AspCore.Utilities.Mapper.Concrete
{
   public class DefaultSetter<TSource, TDestination> : SetterBase<TSource, TDestination>
        where TDestination : class, new()
        where TSource : class, new()
    {
        public override void SetValue(TSource source, TDestination destination, PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            var value = sourceProperty.GetValue(source, null);
            if (value != null)
            {
                var instance = Activator.CreateInstance(destinationProperty.PropertyType);
                instance = SetProperties(value, instance);
                destinationProperty.SetValue(destination, instance, null);
            }
        }
    }
}
