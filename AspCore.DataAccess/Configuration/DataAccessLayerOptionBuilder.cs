using AspCore.ConfigurationAccess.Abstract;
using AspCore.ConfigurationAccess.Concrete;
using AspCore.DataAccess.Abstract;
using AspCore.DataAccess.EntityFramework;
using AspCore.DataAccess.General;
using AspCore.Entities.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AspCore.DataAccess.Configuration
{
    public class DataAccessLayerOptionBuilder : ConfigurationOption
    {
        public DataAccessLayerOptionBuilder(IServiceCollection services) : base(services)
        {
        }

        /// <summary>
        /// DataAccesLayer IDataAccessLayerOption tipinde class bilgileri girilirek configure edilebilir yada configuration bilgileri girilerek otomatik configurasyon çalıştırılabilir.
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="services"></param>
        /// <param name="option"></param>
        public void ConfigureDataAccessLayer<TDbContext, TOption>(string configurationKey, Action<TOption> option = null)
        where TDbContext : CoreDbContext
            where TOption : class, IDataAccessLayerOption, new()
        {
            TOption dataAccessLayerOption = new TOption();
            option?.Invoke(dataAccessLayerOption);

            try
            {
                DatabaseType databaseType = DatabaseType.MSSQL;

                services.AddDbContext<TDbContext>((sp, options) =>
                {

                    dataAccessLayerOption = GetConfigurationOption<TOption>(sp, configurationKey);

                    if (dataAccessLayerOption == null)
                    {
                        throw new Exception(DALConstants.DALErrorMessages.DAL_CONFIGURATION_ERROR_OCCURRED);
                    }

                    if (dataAccessLayerOption.DatabaseSetting != null)
                    {
                        databaseType = dataAccessLayerOption.DatabaseSetting.DatabaseType;

                        if (databaseType == DatabaseType.MSSQL)
                        {
                            options.UseSqlServer(dataAccessLayerOption.DatabaseSetting.MSSQL_ConnectionString);
                        }
                        else if (databaseType == DatabaseType.MySQL)
                        {
                            options.UseMySql(
                                    dataAccessLayerOption.DatabaseSetting.MySQL_ConnectionString,
                                    new MySqlServerVersion(new Version(8, 0, 21)), 
                                    mySqlOptions => mySqlOptions
                                        .CharSetBehavior(CharSetBehavior.NeverAppend))
                                .EnableSensitiveDataLogging()
                                .EnableDetailedErrors();
                            //dataAccessLayerOption.DatabaseSetting.MySQL_ConnectionString;
                        }
                        else
                        {
                            //dataAccessLayerOption.DatabaseSetting.Oracle_ConnectionString;
                        }
                    }
                });

                var transactionBuilder = services.FirstOrDefault(d => d.ServiceType == typeof(ITransactionBuilder));
                if (transactionBuilder == null)
                {
                    services.AddScoped(typeof(ITransactionBuilder), sp =>
                    {
                        dataAccessLayerOption = GetConfigurationOption<TOption>(sp, configurationKey);

                        if (dataAccessLayerOption == null)
                        {
                            throw new Exception(DALConstants.DALErrorMessages.DAL_CONFIGURATION_ERROR_OCCURRED);
                        }

                        if (dataAccessLayerOption.DataLayerType == EnumDataLayerType.EntityFramework)
                        {
                            var context = sp.GetRequiredService<TDbContext>();
                            return new EfTransactionBuilder<TDbContext>(context);
                        }

                        return null;
                    });
                }
            }
            catch
            {
                throw new Exception(DALConstants.DALErrorMessages.DAL_CONFIGURATION_SETTING_ERROR_OCCURRED);
            }
        }

        private TOption GetConfigurationOption<TOption>(IServiceProvider serviceProvider, string configurationKey)
              where TOption : class, IDataAccessLayerOption, new()
        {
            if (!string.IsNullOrEmpty(configurationKey))
            {
                var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
                if (httpContextAccessor == null)
                {
                    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                }
                //configuration helper ile setting

                IConfigurationAccessor configurationHelper = serviceProvider.GetRequiredService<IConfigurationAccessor>();
                if (configurationHelper == null)
                {
                    throw new Exception(ConfigurationHelperConstants.ErrorMessages.CONFIGURATION_HELPER_NOT_FOUND);
                }

                return configurationHelper.GetValueByKey<TOption>(configurationKey);
            }

            return null;
        }
        public void ConfigureDataAccessLayer<TDbContext>(string configurationKey, Action<DataAccessLayerOption> option = null)
        where TDbContext : CoreDbContext
        {
            ConfigureDataAccessLayer<TDbContext, DataAccessLayerOption>(configurationKey, option);
        }
    }
}
