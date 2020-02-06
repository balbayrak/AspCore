using System;
using System.Reflection;

namespace AspCore.Utilities.Mapper.Concrete
{
   public class EnumSetter<TSource,TDestination>:SetterBase<TSource,TDestination>
        where TDestination : class, new() 
        where TSource : class, new()
   {
       public override void SetValue(TSource source, TDestination destination, PropertyInfo sourceProperty, PropertyInfo destinationProperty)
       {
           object value = sourceProperty.GetValue(source, null);
           Type enumType = destinationProperty.PropertyType.IsEnum? destinationProperty.PropertyType: Nullable.GetUnderlyingType(destinationProperty.PropertyType);
           object enumValue = Enum.ToObject(enumType, value);
           destinationProperty.SetValue(destination, enumValue, null);
        }
    }
}
