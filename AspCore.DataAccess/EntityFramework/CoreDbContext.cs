using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using AspCore.DataAccess.EntityFramework.Mapping;
using AspCore.Entities.EntityType;
using AspCore.Extension;
using Microsoft.Extensions.Logging;
using AspCore.DataAccess.EntityFramework.History;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace AspCore.DataAccess.EntityFramework
{
    public abstract class CoreDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CoreDbContext()
        {

        }
        public CoreDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid activeUserId
        {
            get
            {
                return new Guid(_httpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.ACTIVE_USER_ID));
            }
        }

        //   public virtual DbSet<AutoHistory> AutoHistories { get; set; }

        public virtual void OnConfiguringDbContext(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.EnableSensitiveDataLogging();
#if DEBUG
                base.OnConfiguring(optionsBuilder.UseLoggerFactory(ContextLoggerFactory));
#endif
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
            // enable auto history functionality.
            modelBuilder.EnableAutoHistory<AspCoreAutoHistory>(o =>
            {
                o.ChangedMaxLength = 5000;
            });

            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.DefinedTypes).Select(x => x.AsType());

            var typesToRegister = types.Where(type => type.BaseType != null && (type.BaseType.IsGenericType && (type.Name.Contains(nameof(AspCoreAutoHistory)) || type.BaseType.GetGenericTypeDefinition() == typeof(BaseMap<>) || type.BaseType.GetGenericTypeDefinition() == typeof(EntityMap<>))));

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
            ModifyBaseEntities();

            if (IsAutoHistoryEntity())
            {
                this.EnsureAutoHistory(() => new AspCoreAutoHistory()
                {
                    ActiveUserID = activeUserId
                });
            }

            var result = base.SaveChanges();

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ModifyBaseEntities();

            if (IsAutoHistoryEntity())
            {
                this.EnsureAutoHistory(() => new AspCoreAutoHistory()
                {
                    ActiveUserID = activeUserId
                });
            }

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            return result;
        }

        private bool IsAutoHistoryEntity()
        {
            return ChangeTracker.Entries()
               .Any(x => (x.Entity is IAutoHistory)
                   && (x.State == EntityState.Modified || x.State == EntityState.Deleted));
        }

        private void ModifyBaseEntities()
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
                            ((IBaseEntity)entry.Entity).LastUpdatedUserId = activeUserId;
                            ((IBaseEntity)entry.Entity).CreatedDate = now;
                            ((IBaseEntity)entry.Entity).IsDeleted = false;
                        }
                    }
                    else
                    {
                        if (entry.Entity is IBaseEntity)
                        {
                            ((IBaseEntity)entry.Entity).LastUpdatedUserId = activeUserId;
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
        }
    }
}

