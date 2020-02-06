using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Dependency.Abstract;

namespace AspCore.Dependency.Concrete
{
    public class ServicesByNameBuilder<TService>
    {
        private readonly IServiceCollection _services;

        private readonly IDictionary<string, Type> _registrations = new Dictionary<string, Type>();
        private ServiceLifetime _serviceLifetime { get; set; }

        public ServicesByNameBuilder(IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            _services = services;
            _serviceLifetime = serviceLifetime;
        }

        public ServicesByNameBuilder<TService> Add(string name, Type implementationType)
        {
            var descriptor = new ServiceDescriptor(implementationType, implementationType, _serviceLifetime);
            _services.Add(descriptor);
            _registrations.Add(name, implementationType);
            return this;
        }

        /// <summary>
        /// Add by implementation type name
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public ServicesByNameBuilder<TService> Add(Type implementationType)
        {
            var descriptor = new ServiceDescriptor(implementationType, implementationType, _serviceLifetime);
            _services.Add(descriptor);
            _registrations.Add(implementationType.Name, implementationType);
            return this;
        }

        public ServicesByNameBuilder<TService> Add<TImplementation>(string name)
            where TImplementation : TService
        {
            return Add(name, typeof(TImplementation));
        }

        public void Build()
        {
            var registrations = _registrations;
            _services.AddTransient<IServiceByNameFactory<TService>>(s => new ServiceByNameFactory<TService>(registrations));
        }
    }
}
