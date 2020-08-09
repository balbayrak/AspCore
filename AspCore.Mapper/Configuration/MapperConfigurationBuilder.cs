using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspCore.Mapper.Abstract;
using AspCore.Mapper.Concrete;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspCore.Mapper.Configuration
{
    public class MapperConfigurationBuilder : IDisposable
    {
        private readonly IServiceCollection _services;
        public MapperConfigurationBuilder(IServiceCollection services)
        {
            _services = services;
            SetMapper();
        }

        public void SetMapper()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes());
            var mappingConfig = new MapperConfigurationExpression();
            
            mappingConfig.CreateProfile("AspCoreProfile", config =>
            {
                var entities = MapFroms(allTypes);
                foreach (var mapperEntity in entities)
                {
                    config.CreateMap(mapperEntity.Source, mapperEntity.Destination);
                    config.CreateMap(mapperEntity.Destination, mapperEntity.Source);
                }
                var customMap = CustomMappings(allTypes);
                foreach (var map in customMap)
                {
                    map.CreateMappings(config);
                }
            });
            var mapperConfig = new MapperConfiguration(mappingConfig);
            var mapper = mapperConfig.CreateMapper();
            _services.AddSingleton(mapper);
            _services.AddSingleton<IAutoObjectMapper, AutoObjectMapper>();

        }
        private static IEnumerable<MapperEntity> MapFroms(IEnumerable<Type> types)
        {

            var entities = (from t in types
                            from i in t.GetTypeInfo().GetInterfaces()
                            where i.GetTypeInfo().IsGenericType &&
                                  i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                                  !t.GetTypeInfo().IsAbstract &&
                                  !t.GetTypeInfo().IsInterface
                            select new MapperEntity { Source = i.GetGenericArguments()[0], Destination = t });
            return entities;
        }
        private static IEnumerable<ICustomMap> CustomMappings(IEnumerable<Type> types)
        {
            var customMaps = from t in types
                             from i in t.GetTypeInfo().GetInterfaces()
                             where typeof(ICustomMap).GetTypeInfo().IsAssignableFrom(t) &&
                                   !t.GetTypeInfo().IsAbstract &&
                                   !t.GetTypeInfo().IsInterface
                             select (ICustomMap)Activator.CreateInstance(t);
            return customMaps;
        }

        public void Dispose()
        {
        }
    }
}
