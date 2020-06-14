using AspCore.Entities.EntityType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspCore.DataAccess.EntityFramework.Mapping
{
    public abstract class EntityKeyMap<TEntity,TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : CoreEntity
    {
        public abstract Expression<Func<TEntity, object>> keyExpression { get; }
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(keyExpression);

            builder.Property(t => t.Id).HasColumnName("ID").HasDefaultValueSql("newid()").ValueGeneratedOnAdd();
        }
    }
}
