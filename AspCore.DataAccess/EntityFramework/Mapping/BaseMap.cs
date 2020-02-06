using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspCore.Entities.EntityType;

namespace AspCore.DataAccess.EntityFramework.Mapping
{
    public abstract class BaseMap<T> : EntityMap<T>
     where T : BaseEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(t => t.IsDeleted).HasColumnName("IS_DELETED").IsRequired().HasDefaultValue(false);

            builder.Property(t => t.LastUpdateDate).HasColumnName("LAST_UPDATE_TIME").HasColumnType("datetime").IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");

            builder.Property(t => t.CreatedDate).HasColumnName("CREATED_DATE").HasColumnType("datetime").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            
            builder.Property(t => t.LastUpdatedUserId).HasColumnName("LAST_UPDATED_USERID").IsRequired();

        }
    }
}
