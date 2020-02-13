using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.ConfigurationAccess.Concrete;
using AspCore.DataAccess.Abstract;
using AspCore.DataAccess.EntityFramework;
using AspCore.DataAccess.General;
using AspCore.Entities.Configuration;

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
                string connectionString = string.Empty;
                DatabaseType databaseType = DatabaseType.MSSQL;
                ServiceProvider serviceProvider = _services.BuildServiceProvider();

                if (!string.IsNullOrEmpty(configurationKey))
                {
                    var httpContextAccessor = _services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
                    if (httpContextAccessor == null)
                    {
                        _services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    }
                    //configuration helper ile setting

                    IConfigurationAccessor configurationHelper = serviceProvider.GetRequiredService<IConfigurationAccessor>();
                    if (configurationHelper == null)
                    {
                        throw new Exception(ConfigurationHelperConstants.ErrorMessages.CONFIGURATION_HELPER_NOT_FOUND);
                    }

                    dataAccessLayerOption = configurationHelper.GetValueByKey<TOption>(configurationKey);
                }

                if (dataAccessLayerOption == null)
                {
                    throw new Exception(DALConstants.DALErrorMessages.DAL_CONFIGURATION_ERROR_OCCURRED);
                }

                if (dataAccessLayerOption.DatabaseSetting != null)
                {
                    databaseType = dataAccessLayerOption.DatabaseSetting.DatabaseType;

                    if (databaseType == DatabaseType.MSSQL)
                    {
                        connectionString = dataAccessLayerOption.DatabaseSetting.MSSQL_ConnectionString;

                    }
                    else if (databaseType == DatabaseType.MySQL)
                    {
                        connectionString = dataAccessLayerOption.DatabaseSetting.MySQL_ConnectionString;
                    }
                    else
                    {
                        connectionString = dataAccessLayerOption.DatabaseSetting.Oracle_ConnectionString;
                    }
                }


                if (dataAccessLayerOption.DatabaseSetting.DataBaseTransaction == null)
                {
                    ConfigureDataContext<TDbContext>(connectionString, databaseType);
                }
                else
                {
                    ConfigureDataContextWithTransaction<TDbContext>(connectionString, databaseType, dataAccessLayerOption.DatabaseSetting.DataBaseTransaction.isolationLevel);
                }

                var transactionBuilder = _services.FirstOrDefault(d => d.ServiceType == typeof(ITransactionBuilder));
                if (transactionBuilder == null)
                {
                    if (dataAccessLayerOption.DataLayerType == EnumDataLayerType.EntityFramework)
                    {
                        _services.AddScoped(typeof(ITransactionBuilder), typeof(EfTransactionBuilder<TDbContext>));
                    }
                }

            }
            catch
            {
                throw new Exception(DALConstants.DALErrorMessages.DAL_CONFIGURATION_SETTING_ERROR_OCCURRED);
            }
        }

        public void ConfigureDataAccessLayer<TDbContext>(string configurationKey, Action<DataAccessLayerOption> option = null)
        where TDbContext : CoreDbContext
        {
            ConfigureDataAccessLayer<TDbContext, DataAccessLayerOption>(configurationKey, option);
        }

        private void ConfigureDataContext<TDbContext>(string connectionString, DatabaseType dataBaseType)
          where TDbContext : CoreDbContext
        {
            if (dataBaseType == DatabaseType.MSSQL)
            {
                _services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));
            }
            else if (dataBaseType == DatabaseType.MySQL)
            {
            }
            else
            {
            }
        }

        private void ConfigureDataContextWithTransaction<TDbContext>(string connectionString, DatabaseType dataBaseType, IsolationLevel level = IsolationLevel.ReadUncommitted)
               where TDbContext : CoreDbContext
        {
            if (dataBaseType == DatabaseType.MSSQL)
            {

                //First, configure the SqlConnection and open it
                //This is done for every request/response
                _services.AddScoped<DbConnection>((serviceProvider) =>
                {
                    var dbConnection = new SqlConnection(connectionString);
                    dbConnection.Open();
                    return dbConnection;
                });

                //Start a new transaction based on the SqlConnection
                //This is done for every request/response
                _services.AddScoped<DbTransaction>((serviceProvider) =>
                {
                    var dbConnection = serviceProvider
                        .GetService<DbConnection>();

                    return dbConnection.BeginTransaction(level);
                });

                //Create DbOptions for the DbContext, use the DbConnection
                //This is done for every request/response
                _services.AddScoped<DbContextOptions>((serviceProvider) =>
                {
                    var dbConnection = serviceProvider.GetService<DbConnection>();
                    return new DbContextOptionsBuilder<TDbContext>()
                        .UseSqlServer(dbConnection)
                        .Options;
                });



                //Finally, create the DbContext, using the transaction
                //This is done for every request/response
                _services.AddScoped<TDbContext>((serviceProvider) =>
                {
                    var transaction = serviceProvider.GetService<DbTransaction>();
                    var options = serviceProvider.GetService<DbContextOptions>();
                    TDbContext context = (TDbContext)Activator.CreateInstance(typeof(TDbContext), new object[] { options });
                    context.Database.UseTransaction(transaction);
                    return context;
                });

            }
            else if (dataBaseType == DatabaseType.MySQL)
            {
            }
            else
            {
            }
        }
    }
}
