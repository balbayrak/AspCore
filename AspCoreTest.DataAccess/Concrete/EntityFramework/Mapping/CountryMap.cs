using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework.Mapping;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework.Mapping
{
    public class CountryMap : BaseMap<Country>
    {
        public override void Configure(EntityTypeBuilder<Country> builder)
        {
            base.Configure(builder);

            builder.ToTable("COUNTRIES");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("NAME")
                .HasMaxLength(50);
        }
    }
}
