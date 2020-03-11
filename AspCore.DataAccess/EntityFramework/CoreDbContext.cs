using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using AspCore.DataAccess.EntityFramework.Mapping;
using AspCore.Entities.EntityType;
using AspCore.Extension;
using Microsoft.Extensions.Logging;

namespace AspCore.DataAccess.EntityFramework
{
    public abstract class CoreDbContext : DbContext
    {

        public CoreDbContext()
        {
        }
        public CoreDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual void OnConfiguringDbContext(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.EnableSensitiveDataLogging();
                base.OnConfiguring(optionsBuilder.UseLoggerFactory(ContextLoggerFactory));
                OnConfiguringDbContext(optionsBuilder);
            }
        }

        public static readonly ILoggerFactory ContextLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information)
                    .AddDebug();
            });
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.DefinedTypes).Select(x => x.AsType());

            var typesToRegister = types.Where(type => type.BaseType != null && (type.BaseType.IsGenericType && (type.BaseType.GetGenericTypeDefinition() == typeof(BaseMap<>) || type.BaseType.GetGenericTypeDefinition() == typeof(EntityMap<>))));

            foreach (var type in typesToRegister)
            {
                if (type.Name.Contains("BaseMap") || type.Name.Contains("EntityMap")) continue;
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
            modelBuilder.AddGlobalDeletedFilter();
        }
        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => (x.Entity is IEntity || x.Entity is IBaseEntity)
                    && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var entry in modifiedEntries)
            {
                IEntity entity = entry.Entity as IEntity;

                if (entity != null)
                {
                    DateTime now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                        if (entry.Entity is IBaseEntity)
                        {
                            ((IBaseEntity)entry.Entity).CreatedDate = now;
                            ((IBaseEntity)entry.Entity).IsDeleted = false;
                        }
                    }
                    else
                    {
                        if (entry.Entity is IBaseEntity)
                        {
                            base.Entry((IBaseEntity)entry.Entity).Property(x => x.CreatedDate).IsModified = false;
                            ((IBaseEntity)entry.Entity).LastUpdateDate = now;
                            if (entry.State == EntityState.Deleted)
                            {
                                entry.State = EntityState.Modified;
                                ((IBaseEntity)entry.Entity).IsDeleted = true;
                            }
                        }
                    }
                }
            }

            return base.SaveChanges();

        }
    }
}
