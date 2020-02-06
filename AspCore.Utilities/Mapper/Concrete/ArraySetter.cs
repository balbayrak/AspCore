using System;
using System.Reflection;

namespace AspCore.Utilities.Mapper.Concrete
{
    public class ArraySetter<TSource, TDestination> : SetterBase<TSource, TDestination>
        where TDestination : class, new()
        where TSource : class, new()
    {
        public override void SetValue(TSource source, TDestination destination, PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            var sourceValue = sourceProperty.GetValue(source, null);
            var sourceArray = (sourceValue as Array);
            Array destinationArray = Array.CreateInstance(destinationProperty.PropertyType.GetElementType(), sourceArray.Length);
            for (int index = 0; index < destinationArray.Length; index++)
            {
                if (destinationProperty.PropertyType.IsClass)
                {

                    var instance = Activator.CreateInstance(destinationProperty.PropertyType.GetElementType());
                    instance = SetProperties(sourceArray.GetValue(index), instance);
                    destinationArray.SetValue(instance, index);

                }
                else
                {
                    object value = sourceArray.GetValue(index);
                    destinationArray.SetValue(value, index);
                }
            }
            destinationProperty.SetValue(destination, destinationArray, null);
        }
    }
}
