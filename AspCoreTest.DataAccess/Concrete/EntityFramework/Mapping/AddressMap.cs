using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework.Mapping;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework.Mapping
{
    public class AddressMap : BaseMap<Address>
    {
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            base.Configure(builder);

            builder.ToTable("ADDRESSES");

            builder.Property(e => e.CityId).HasColumnName("CITY_ID");

            builder.Property(e => e.FullAddress)
                .IsRequired()
                .HasColumnName("FULL_ADDRESS")
                .HasMaxLength(500);

            builder.HasOne(d => d.City)
                .WithMany(p => p.Address)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_ADDRESS_CITY");

        }
    }
}
