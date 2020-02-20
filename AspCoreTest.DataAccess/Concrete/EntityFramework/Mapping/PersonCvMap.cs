using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework.Mapping;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework.Mapping
{
    public class PersonCvMap : BaseMap<PersonCv>
    {
        public override void Configure(EntityTypeBuilder<PersonCv> builder)
        {
            base.Configure(builder);

            builder.ToTable("PERSON_CVS");

            builder.Property(e => e.DocumentUrl)
                .IsRequired()
                .HasColumnName("DOCUMENT_URL")
                .HasMaxLength(100);


            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("NAME")
                .HasMaxLength(50);

            builder.Property(e => e.PersonId).HasColumnName("PERSON_ID");

            builder.HasOne(d => d.Person)
                .WithMany(p => p.PersonCv)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK_PERSON_CV_PERSON");

        }
    }
}
