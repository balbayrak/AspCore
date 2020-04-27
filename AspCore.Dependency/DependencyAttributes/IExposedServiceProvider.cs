using System;

namespace AspCore.Dependency.DependencyAttributes
{
    public interface IExposedServiceProvider
    {
        Type[] GetExposedServiceTypes(Type targetType);
    }
}
