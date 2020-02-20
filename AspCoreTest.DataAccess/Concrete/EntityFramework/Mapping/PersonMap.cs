using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework.Mapping;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework.Mapping
{
    public class PersonMap : BaseMap<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder);

            builder.ToTable("PERSONS");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("NAME")
                .HasMaxLength(50);

            builder.Property(e => e.Surname)
                .IsRequired()
                .HasColumnName("SURNAME")
                .HasMaxLength(50);

        }
    }
}
