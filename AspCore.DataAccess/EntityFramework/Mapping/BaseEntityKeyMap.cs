using AspCore.Entities.EntityType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspCore.DataAccess.EntityFramework.Mapping
{
    public abstract class BaseEntityKeyMap<TEntity,TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public abstract Expression<Func<TEntity, object>> keyExpression { get; }
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(keyExpression);

            builder.Property(t => t.Id).HasColumnName("ID").HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

            builder.Property(t => t.IsDeleted).HasColumnName("IS_DELETED").IsRequired().HasDefaultValue(false);

            builder.Property(t => t.LastUpdateDate).HasColumnName("LAST_UPDATE_TIME").HasColumnType("datetime").IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");

            builder.Property(t => t.CreatedDate).HasColumnName("CREATED_DATE").HasColumnType("datetime").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            
            builder.Property(t => t.LastUpdatedUserId).HasColumnName("LAST_UPDATED_USERID").IsRequired();

        }
    }
}
