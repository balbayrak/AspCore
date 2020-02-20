using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework.Mapping;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework.Mapping
{
    public class PersonAddressMap : EntityMap<PersonAddress>
    {
        public override void Configure(EntityTypeBuilder<PersonAddress> builder)
        {
            base.Configure(builder);

            builder.ToTable("PERSON_ADDRESSES");

            builder.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

            builder.Property(e => e.PersonId).HasColumnName("PERSON_ID");

            builder.HasOne(d => d.Address)
                .WithMany(p => p.PersonAddress)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_PERSON_ADDRESS_ADDRESS");

            builder.HasOne(d => d.Person)
                .WithMany(p => p.PersonAddress)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK_PERSON_ADDRESS_PERSON");

        }
    }
}
