using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AspCore.Entities.EntityType;
using Microsoft.EntityFrameworkCore;

namespace AspCore.Extension
{
    public static class ModelBuilderExt
    {
        public static void AddGlobalDeletedFilter(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.SetDeleteFilter(entityType.ClrType);
                }
            }
        }
        public static void SetDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetDeleteFilterMethod.MakeGenericMethod(entityType)
                .Invoke(null, new object[] { modelBuilder });
        }

        static readonly MethodInfo SetDeleteFilterMethod = typeof(ModelBuilderExt)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(t => t.IsGenericMethod && t.Name == "SetDeleteFilter");

        public static void SetDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, IBaseEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
