using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework.Mapping;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework.Mapping
{
    public class PersonRoleMap : EntityMap<PersonRole>
    {
        public override void Configure(EntityTypeBuilder<PersonRole> builder)
        {
            base.Configure(builder);

            builder.ToTable("PERSON_ROLES");


            builder.Property(e => e.PersonId).HasColumnName("PERSON_ID");

            builder.Property(e => e.RoleId).HasColumnName("ROLE_ID");

            builder.HasOne(d => d.Person)
                .WithMany(p => p.PersonRole)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK_PERSON_ROLE_PERSON");

            builder.HasOne(d => d.Role)
                .WithMany(p => p.PersonRole)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_PERSON_ROLE_ROLE");

        }
    }
}
