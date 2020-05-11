using AspCore.DataAccess.EntityFramework.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspCore.DataAccess.EntityFramework.History
{
    public class AspCoreAutoHistoryMap : IEntityTypeConfiguration<AspCoreAutoHistory>
    {
        public virtual void Configure(EntityTypeBuilder<AspCoreAutoHistory> builder)
        {
            builder.ToTable("AspCoreAutoHistory");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.RowId)
                .IsRequired();

            builder.Property(e => e.TableName)
              .IsRequired()
              .HasMaxLength(250);

            builder.Property(e => e.Changed)
               .HasMaxLength(5000)
              .IsRequired();

            builder.Property(e => e.Kind)
                .IsRequired();

            builder.Property(e => e.Created).IsRequired().HasDefaultValueSql("getdate()");

            builder.Property(e => e.ActiveUserID)
                .IsRequired();
        }
    }
}
