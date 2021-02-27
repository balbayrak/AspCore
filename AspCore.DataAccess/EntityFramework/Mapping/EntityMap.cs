using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspCore.Entities.EntityType;

namespace AspCore.DataAccess.EntityFramework.Mapping
{
    public abstract class EntityMap<T> : IEntityTypeConfiguration<T>
    where T : CoreEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("ID").HasDefaultValueSql("UUID()").ValueGeneratedOnAdd();
        }
    }
}
