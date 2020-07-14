using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using AspCore.Dependency.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Dependency.DependencyAttributes;
using AspCore.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspCore.Dependency.Configuration
{
    public class DependencyOptionBuilder : ConfigurationOption, IDisposable
    {
        public DependencyOptionBuilder(IServiceCollection services) : base(services)
        {
        }
        public void AutoBind(Action<DependencyOption> option = null)
        {
            string nameSpaceStr = null;

            if (option != null)
            {
                DependencyOption dependencyOption = new DependencyOption();
                option(dependencyOption);
                nameSpaceStr = dependencyOption.namespaceStr;
            }

            BindType<ITransientType>(ServiceLifetime.Transient, nameSpaceStr);
            BindType<IScopedType>(ServiceLifetime.Scoped, nameSpaceStr);
            BindType<ISingletonType>(ServiceLifetime.Singleton, nameSpaceStr);
        }

        public void AutoBindModules()
        {
            IEnumerable<TypeMap> maps = TypeMapHelper.GetTypeMaps<IDependencyModule>(AppDomain.CurrentDomain.GetAssemblies());

            foreach (var typeMap in maps)
            {
                var implementationType = typeMap.ImplementationType;

                var module = (AspCoreDependencyModule)Activator.CreateInstance(implementationType, services);
                module.ConfigureServices();
            }
        }

        public void Bind<TInterface>(Action<DependencyOption> option)
        {
            DependencyOption dependencyOption = new DependencyOption();
            option(dependencyOption);

            BindType<TInterface>(dependencyOption.serviceLifetime, dependencyOption.namespaceStr);
        }

        public void Bind<TInterface, TConcrete>(Action<DependencyOption> option)
        {
            DependencyOption dependencyOption = new DependencyOption();
            option(dependencyOption);

            var descriptor = new ServiceDescriptor(typeof(TInterface), typeof(TConcrete), dependencyOption.serviceLifetime);

            var oldDescription = services.FirstOrDefault(t => t.ServiceType.Equals(typeof(TInterface)));
            if (oldDescription != null)
            {
                services.Remove(oldDescription);
            }

            var oldDescriptionImp = services.FirstOrDefault(t => (t.ImplementationType != null && t.ImplementationType.Equals(typeof(TConcrete))));
            if (oldDescriptionImp != null)
            {
                services.Remove(oldDescriptionImp);
            }

            services.Add(descriptor);
        }

        public void AddDependencyModule<T>()
            where T : AspCoreDependencyModule
        {
            var module = (T)Activator.CreateInstance(typeof(T), services);
            module.ConfigureServices();
        }

        private void BindType<TInterface>(ServiceLifetime lifeTime = ServiceLifetime.Scoped, string namespaceStr = null)
        {
            IEnumerable<TypeMap> maps = TypeMapHelper.GetTypeMaps<TInterface>(AppDomain.CurrentDomain.GetAssemblies(), namespaceStr, true);

            foreach (var typeMap in maps)
            {

                if (!typeMap.ExposedTypes.Any())
                {
                    foreach (var serviceType in typeMap.ServiceTypes)
                    {
                        var implementationType = typeMap.ImplementationType;

                        if (!implementationType.IsAssignableTo(serviceType))
                        {
                            throw new InvalidOperationException($@"Type ""{implementationType.ToFriendlyName()}"" is not assignable to ""${serviceType.ToFriendlyName()}"".");
                        }

                        var descriptor = new ServiceDescriptor(serviceType, implementationType, lifeTime);

                        var oldDescription = services.FirstOrDefault(t => t.ServiceType == typeof(TInterface));
                        if (oldDescription != null)
                        {
                            services.Remove(oldDescription);
                        }

                        var oldDescriptionImp = services.FirstOrDefault(t => t.ServiceType == implementationType);
                        if (oldDescriptionImp != null)
                        {
                            services.Remove(oldDescriptionImp);
                        }

                        services.Add(descriptor);
                    }
                }
                else
                {
                    foreach (var serviceType in typeMap.ExposedTypes)
                    {
                        var descriptor = ServiceDescriptor.Describe(serviceType, typeMap.ImplementationType, lifeTime);
                        services.Replace(descriptor);
                    }
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
