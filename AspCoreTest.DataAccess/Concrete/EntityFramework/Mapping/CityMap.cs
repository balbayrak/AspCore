using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework.Mapping;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework.Mapping
{
    public class CityMap : BaseMap<City>
    {
        public override void Configure(EntityTypeBuilder<City> builder)
        {
            base.Configure(builder);

            builder.ToTable("CITIES");

            builder.Property(e => e.CountryId).HasColumnName("COUNTRY_ID");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("NAME")
                .HasMaxLength(50);

            builder.HasOne(d => d.Country)
                .WithMany(p => p.City)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_CITY_COUNTRY");
        }
    }
}
