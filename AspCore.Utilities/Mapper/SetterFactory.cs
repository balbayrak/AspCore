using System;
using System.Collections.Generic;
using System.Reflection;
using AspCore.Utilities.Mapper.Concrete;

namespace AspCore.Utilities.Mapper
{
    internal static class SetterFactory
    {
        public static SetterBase<TS, TD> GetSetter<TS, TD>(TS source,PropertyInfo destinationProperty,PropertyInfo sourceProperty) 
            where TS : class, new()
            where TD : class, new()
        {
            SetterBase<TS, TD> setter;
            if (!destinationProperty.PropertyType.IsClass)
            {
                object value = sourceProperty.GetValue(source, null);
                Type enumType = Nullable.GetUnderlyingType(destinationProperty.PropertyType);
                if (sourceProperty.PropertyType.IsEnum || enumType != null && enumType.IsEnum && value != null)
                {
                    setter = new EnumSetter<TS, TD>();
                }
                else
                {
                    setter = new SetterBase<TS, TD>();
                }
            }
            else if (IsSetableListOrArrayType(destinationProperty))
            {
                setter = new SetterBase<TS, TD>();
            }
            else if (destinationProperty.PropertyType.IsArray)
            {
                setter = new ArraySetter<TS, TD>();
            }
            else if (destinationProperty.PropertyType.IsGenericType)
            {
                setter = new GenericSetter<TS, TD>();
            }
            else
            {
                setter = new DefaultSetter<TS, TD>();
            }
            return setter;
        }

        private static bool IsSetableListOrArrayType(PropertyInfo sourceProperty)
        {
            List<Type> typeOfSetableArray = new List<Type>()
            {
                typeof(byte[]),
                typeof(string[]),
                typeof(String),
                typeof(Guid[]),
                typeof(List<Guid>),
                typeof(List<string>),
                typeof(List<String>),
            };
            return typeOfSetableArray.Contains(sourceProperty.PropertyType);
        }
    }
}
