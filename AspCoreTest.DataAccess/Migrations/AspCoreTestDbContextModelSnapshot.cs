﻿// <auto-generated />
using System;
using AspCoreTest.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AspCoreTest.DataAccess.Migrations
{
    [DbContext(typeof(AspCoreTestDbContext))]
    partial class AspCoreTestDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("AspCore.DataAccess.EntityFramework.History.AspCoreAutoHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<Guid>("ActiveUserID")
                        .HasColumnType("char(36)");

                    b.Property<string>("Changed")
                        .HasMaxLength(5000)
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Kind")
                        .HasColumnType("int");

                    b.Property<string>("RowId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.HasKey("Id");

                    b.ToTable("AspCoreAutoHistory");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("UUID()");

                    b.Property<Guid>("CityId")
                        .HasColumnType("char(36)")
                        .HasColumnName("CITY_ID");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("CREATED_DATE")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("FullAddress")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("FULL_ADDRESS");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("IS_DELETED");

                    b.Property<DateTime>("LastUpdateDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("LAST_UPDATE_TIME")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<Guid>("LastUpdatedUserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("LAST_UPDATED_USERID");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("ADDRESSES");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("UUID()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("DESCRIPTION");

                    b.HasKey("Id");

                    b.ToTable("ADMINS");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.City", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("UUID()");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("char(36)")
                        .HasColumnName("COUNTRY_ID");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("CREATED_DATE")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("IS_DELETED");

                    b.Property<DateTime>("LastUpdateDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("LAST_UPDATE_TIME")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<Guid>("LastUpdatedUserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("LAST_UPDATED_USERID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("NAME");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("CITIES");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("UUID()");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("CREATED_DATE")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("IS_DELETED");

                    b.Property<DateTime>("LastUpdateDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("LAST_UPDATE_TIME")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<Guid>("LastUpdatedUserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("LAST_UPDATED_USERID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("NAME");

                    b.HasKey("Id");

                    b.ToTable("COUNTRIES");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("UUID()");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("CREATED_DATE")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("IS_DELETED");

                    b.Property<DateTime>("LastUpdateDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("LAST_UPDATE_TIME")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<Guid>("LastUpdatedUserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("LAST_UPDATED_USERID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("NAME");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("SURNAME");

                    b.HasKey("Id");

                    b.ToTable("PERSONS");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.PersonAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("UUID()");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("char(36)")
                        .HasColumnName("ADDRESS_ID");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("char(36)")
                        .HasColumnName("PERSON_ID");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("PersonId");

                    b.ToTable("PERSON_ADDRESSES");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.PersonCv", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("UUID()");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("CREATED_DATE")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("DocumentUrl")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("DOCUMENT_URL");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("IS_DELETED");

                    b.Property<DateTime>("LastUpdateDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("LAST_UPDATE_TIME")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<Guid>("LastUpdatedUserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("LAST_UPDATED_USERID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("NAME");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("char(36)")
                        .HasColumnName("PERSON_ID");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("PERSON_CVS");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.PersonRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("UUID()");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("char(36)")
                        .HasColumnName("PERSON_ID");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(36)")
                        .HasColumnName("ROLE_ID");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("RoleId");

                    b.ToTable("PERSON_ROLES");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("UUID()");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("CREATED_DATE")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("IS_DELETED");

                    b.Property<DateTime>("LastUpdateDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("LAST_UPDATE_TIME")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<Guid>("LastUpdatedUserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("LAST_UPDATED_USERID");

                    b.Property<Guid>("Name")
                        .HasColumnType("char(36)")
                        .HasColumnName("NAME");

                    b.HasKey("Id");

                    b.ToTable("ROLES");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Address", b =>
                {
                    b.HasOne("AspCoreTest.Entities.Models.City", "City")
                        .WithMany("Address")
                        .HasForeignKey("CityId")
                        .HasConstraintName("FK_ADDRESS_CITY")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Admin", b =>
                {
                    b.HasOne("AspCoreTest.Entities.Models.Person", "Person")
                        .WithOne("Admin")
                        .HasForeignKey("AspCoreTest.Entities.Models.Admin", "Id")
                        .HasConstraintName("FK_PERSON_ADMIN")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.City", b =>
                {
                    b.HasOne("AspCoreTest.Entities.Models.Country", "Country")
                        .WithMany("City")
                        .HasForeignKey("CountryId")
                        .HasConstraintName("FK_CITY_COUNTRY")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.PersonAddress", b =>
                {
                    b.HasOne("AspCoreTest.Entities.Models.Address", "Address")
                        .WithMany("PersonAddress")
                        .HasForeignKey("AddressId")
                        .HasConstraintName("FK_PERSON_ADDRESS_ADDRESS")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AspCoreTest.Entities.Models.Person", "Person")
                        .WithMany("PersonAddress")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("FK_PERSON_ADDRESS_PERSON")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.PersonCv", b =>
                {
                    b.HasOne("AspCoreTest.Entities.Models.Person", "Person")
                        .WithMany("PersonCv")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("FK_PERSON_CV_PERSON")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.PersonRole", b =>
                {
                    b.HasOne("AspCoreTest.Entities.Models.Person", "Person")
                        .WithMany("PersonRole")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("FK_PERSON_ROLE_PERSON")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AspCoreTest.Entities.Models.Role", "Role")
                        .WithMany("PersonRole")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_PERSON_ROLE_ROLE")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Address", b =>
                {
                    b.Navigation("PersonAddress");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.City", b =>
                {
                    b.Navigation("Address");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Country", b =>
                {
                    b.Navigation("City");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Person", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("PersonAddress");

                    b.Navigation("PersonCv");

                    b.Navigation("PersonRole");
                });

            modelBuilder.Entity("AspCoreTest.Entities.Models.Role", b =>
                {
                    b.Navigation("PersonRole");
                });
#pragma warning restore 612, 618
        }
    }
}
