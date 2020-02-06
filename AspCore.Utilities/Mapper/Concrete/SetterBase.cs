using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspCore.Utilities.Mapper.Concrete
{
    public class SetterBase<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        public virtual void SetValue(TSource source, TDestination destination, PropertyInfo sourceProperty,
            PropertyInfo destinationProperty)
        {
            object value = sourceProperty.GetValue(source, null);
            destinationProperty.SetValue(destination, value, null);
        }

        public TD SetProperties<TS, TD>(TS source, TD destination)
            where TS : class, new()
            where TD : class, new()
        {
            var sourceProperties = source.GetType().GetProperties().ToList();
            var destinationProperties = destination.GetType().GetProperties().ToList();
            foreach (var sourceProperty in sourceProperties)
            {
                SetDestinationFromSource(source, destination, destinationProperties, sourceProperty);
            }
            return destination;
        }

        protected void SetDestinationFromSource<TS, TD>(TS source, TD destination, List<PropertyInfo> destinationProperties, PropertyInfo sourceProperty)
           where TS : class, new()
           where TD : class, new()
        {
            var destinationProperty = destinationProperties.Find(item => item.Name == sourceProperty.Name);

            if (destinationProperty != null)
            {
                try
                {
                    SetterBase<TS, TD> setter=SetterFactory.GetSetter<TS,TD>(source,destinationProperty,sourceProperty);
                    setter.SetValue(source,destination,sourceProperty,destinationProperty);
                }
                catch
                {
                    // ignored
                }
            }
        }

       
    }
}
