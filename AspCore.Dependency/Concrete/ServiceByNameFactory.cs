using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspCore.Dependency.Abstract;

namespace AspCore.Dependency.Concrete
{
    public class ServiceByNameFactory<TService> : IServiceByNameFactory<TService>
    {
        private string _defaultRegistration { get; set; }
        private readonly IDictionary<string, Type> _registrations;

        public ServiceByNameFactory(IDictionary<string, Type> registrations, string defaultRegistration = null)
        {
            _registrations = registrations;
            _defaultRegistration = defaultRegistration;
        }

        public TService GetByName(IServiceProvider serviceProvider, string name)
        {
            Type implementationType = null;

            if (!_registrations.TryGetValue(name, out implementationType))
                throw new ArgumentException($"Service name '{name}' is not registered");

            Dictionary<string, Type> impDict = _registrations.Where(t => t.Value == implementationType).ToDictionary(t => t.Key, t => t.Value);

            if (impDict.Count > 1)
            {
                int index = Array.IndexOf(impDict.Keys.ToArray(), name);
                return (TService)serviceProvider.GetServices(implementationType).ToList()[index];
            }

            return (TService)serviceProvider.GetService(implementationType);
        }

        public string GetDefaultRegistrationName()
        {
            return _defaultRegistration;
        }
    }
}
