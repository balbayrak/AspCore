using System;

namespace AspCore.Dependency.Abstract
{
    public interface IServiceByNameFactory<TService>
    {
        TService GetByName(IServiceProvider serviceProvider, string name);

        string GetDefaultRegistrationName();
    }
}
