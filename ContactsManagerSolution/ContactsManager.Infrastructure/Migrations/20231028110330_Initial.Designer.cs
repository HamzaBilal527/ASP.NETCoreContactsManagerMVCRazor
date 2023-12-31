﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContactsManager.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231028110330_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryId = new Guid("d93c54b2-3d33-4854-a852-f0d2a43d4dde"),
                            CountryName = "Philippines"
                        },
                        new
                        {
                            CountryId = new Guid("3a8d6605-83d0-4158-9560-290829229a5a"),
                            CountryName = "Thailand"
                        },
                        new
                        {
                            CountryId = new Guid("9fb9b388-f673-474e-8f61-189ac5ea07c7"),
                            CountryName = "China"
                        },
                        new
                        {
                            CountryId = new Guid("122177a5-54c1-4b97-873d-cfbf66a61497"),
                            CountryName = "Palestinian Territory"
                        },
                        new
                        {
                            CountryId = new Guid("f79cb2c7-272c-4a67-8b60-a26deb03498d"),
                            CountryName = "China"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Gender")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("PersonName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("RevieveNewsLetter")
                        .HasColumnType("bit");

                    b.Property<string>("TIN")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(8)")
                        .HasDefaultValue("abc12345")
                        .HasColumnName("TaxIdentificationNumber");

                    b.HasKey("PersonId");

                    b.HasIndex("CountryId");

                    b.ToTable("Persons", (string)null);

                    b.HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");

                    b.HasData(
                        new
                        {
                            PersonId = new Guid("fdb709d0-8911-4424-9fca-aca38d69b455"),
                            Address = "4 Parkside Point",
                            CountryId = new Guid("d93c54b2-3d33-4854-a852-f0d2a43d4dde"),
                            DateOfBirth = new DateTime(1989, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mwebsdale0@people.com.cn",
                            Gender = "Female",
                            PersonName = "Marguerite",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("340bcf2a-acf3-4251-85a9-d91afb82c977"),
                            Address = "6 Morningstar Circle",
                            CountryId = new Guid("d93c54b2-3d33-4854-a852-f0d2a43d4dde"),
                            DateOfBirth = new DateTime(1990, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "ushears1@globo.com",
                            Gender = "Female",
                            PersonName = "Ursa",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("e72f5aeb-9b79-4edd-9888-22e3ed604a53"),
                            Address = "73 Heath Avenue",
                            CountryId = new Guid("3a8d6605-83d0-4158-9560-290829229a5a"),
                            DateOfBirth = new DateTime(1995, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "fbowsher2@howstuffworks.com",
                            Gender = "Male",
                            PersonName = "Franchot",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("a209913c-3db2-4689-8a8b-0d06a7870a6a"),
                            Address = "83187 Merry Drive",
                            CountryId = new Guid("3a8d6605-83d0-4158-9560-290829229a5a"),
                            DateOfBirth = new DateTime(1987, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "asarvar3@dropbox.com",
                            Gender = "Male",
                            PersonName = "Angie",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("cd32113b-20d2-4783-95ef-0f713de26616"),
                            Address = "50467 Holy Cross Crossing",
                            CountryId = new Guid("9fb9b388-f673-474e-8f61-189ac5ea07c7"),
                            DateOfBirth = new DateTime(1995, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "ttregona4@stumbleupon.com",
                            Gender = "Gender",
                            PersonName = "Tani",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("b3f536a0-c4ef-49b2-9dbc-ac70a7bf21c2"),
                            Address = "97570 Raven Circle",
                            CountryId = new Guid("9fb9b388-f673-474e-8f61-189ac5ea07c7"),
                            DateOfBirth = new DateTime(1988, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mlingfoot5@netvibes.com",
                            Gender = "Male",
                            PersonName = "Mitchael",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("4f144000-0cbe-4a01-99d3-7d1a9b03da52"),
                            Address = "57449 Brown Way",
                            CountryId = new Guid("122177a5-54c1-4b97-873d-cfbf66a61497"),
                            DateOfBirth = new DateTime(1983, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mjarrell6@wisc.edu",
                            Gender = "Male",
                            PersonName = "Maddy",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("07145af4-5c89-401a-a065-d03c11c35433"),
                            Address = "4 Stuart Drive",
                            CountryId = new Guid("122177a5-54c1-4b97-873d-cfbf66a61497"),
                            DateOfBirth = new DateTime(1998, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "pretchford7@virginia.edu",
                            Gender = "Female",
                            PersonName = "Pegeen",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("680ba1f5-a462-419a-890b-644701541776"),
                            Address = "413 Sachtjen Way",
                            CountryId = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            DateOfBirth = new DateTime(1990, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "hmosco8@tripod.com",
                            Gender = "Male",
                            PersonName = "Hansiain",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("5161fb91-39f3-4c4a-8626-7a0200c8009c"),
                            Address = "484 Clarendon Court",
                            CountryId = new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"),
                            DateOfBirth = new DateTime(1997, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "lwoodwing9@wix.com",
                            Gender = "Male",
                            PersonName = "Lombard",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("864009bb-5c9e-4c72-b3ca-aff9e7e04245"),
                            Address = "2 Warrior Avenue",
                            CountryId = new Guid("f79cb2c7-272c-4a67-8b60-a26deb03498d"),
                            DateOfBirth = new DateTime(1990, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mconachya@va.gov",
                            Gender = "Female",
                            PersonName = "Minta",
                            RevieveNewsLetter = false
                        },
                        new
                        {
                            PersonId = new Guid("b8096b64-8369-4ab9-a60a-ff346a011823"),
                            Address = "9334 Fremont Street",
                            CountryId = new Guid("f79cb2c7-272c-4a67-8b60-a26deb03498d"),
                            DateOfBirth = new DateTime(1987, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "vklussb@nationalgeographic.com",
                            Gender = "Female",
                            PersonName = "Verene",
                            RevieveNewsLetter = false
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.HasOne("Entities.Country", "Country")
                        .WithMany("Persons")
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Navigation("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
