using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework.Mapping;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework.Mapping
{
    public class AdminMap : EntityMap<Admin>
    {
        public override void Configure(EntityTypeBuilder<Admin> builder)
        {
            base.Configure(builder);

            builder.ToTable("ADMINS");

            builder.Property(e => e.Description)
                .IsRequired()
                .HasColumnName("DESCRIPTION")
                .HasMaxLength(100);

            builder.HasOne(d => d.Person)
                .WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.Id)
                .HasConstraintName("FK_PERSON_ADMIN");
        }
    }
}
