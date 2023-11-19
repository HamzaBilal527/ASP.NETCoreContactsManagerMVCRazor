using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManager.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    RevieveNewsLetter = table.Column<bool>(type: "bit", nullable: false),
                    TaxIdentificationNumber = table.Column<string>(type: "varchar(8)", nullable: true, defaultValue: "abc12345")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                    table.CheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");
                    table.ForeignKey(
                        name: "FK_Persons_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("122177a5-54c1-4b97-873d-cfbf66a61497"), "Palestinian Territory" },
                    { new Guid("3a8d6605-83d0-4158-9560-290829229a5a"), "Thailand" },
                    { new Guid("9fb9b388-f673-474e-8f61-189ac5ea07c7"), "China" },
                    { new Guid("d93c54b2-3d33-4854-a852-f0d2a43d4dde"), "Philippines" },
                    { new Guid("f79cb2c7-272c-4a67-8b60-a26deb03498d"), "China" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "PersonName", "RevieveNewsLetter" },
                values: new object[,]
                {
                    { new Guid("5161fb91-39f3-4c4a-8626-7a0200c8009c"), "484 Clarendon Court", new Guid("3a8d6605-83d0-4158-9560-290829229a5a"), new DateTime(1997, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "lwoodwing9@wix.com", "Male", "Lombard", false },
                    { new Guid("680ba1f5-a462-419a-890b-644701541776"), "413 Sachtjen Way", new Guid("d93c54b2-3d33-4854-a852-f0d2a43d4dde"), new DateTime(1990, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "hmosco8@tripod.com", "Male", "Hansiain", false }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "PersonName", "RevieveNewsLetter" },
                values: new object[,]
                {
                    { new Guid("07145af4-5c89-401a-a065-d03c11c35433"), "4 Stuart Drive", new Guid("122177a5-54c1-4b97-873d-cfbf66a61497"), new DateTime(1998, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "pretchford7@virginia.edu", "Female", "Pegeen", false },
                    { new Guid("340bcf2a-acf3-4251-85a9-d91afb82c977"), "6 Morningstar Circle", new Guid("d93c54b2-3d33-4854-a852-f0d2a43d4dde"), new DateTime(1990, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "ushears1@globo.com", "Female", "Ursa", false },
                    { new Guid("4f144000-0cbe-4a01-99d3-7d1a9b03da52"), "57449 Brown Way", new Guid("122177a5-54c1-4b97-873d-cfbf66a61497"), new DateTime(1983, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "mjarrell6@wisc.edu", "Male", "Maddy", false },
                    { new Guid("864009bb-5c9e-4c72-b3ca-aff9e7e04245"), "2 Warrior Avenue", new Guid("f79cb2c7-272c-4a67-8b60-a26deb03498d"), new DateTime(1990, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "mconachya@va.gov", "Female", "Minta", false },
                    { new Guid("a209913c-3db2-4689-8a8b-0d06a7870a6a"), "83187 Merry Drive", new Guid("3a8d6605-83d0-4158-9560-290829229a5a"), new DateTime(1987, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "asarvar3@dropbox.com", "Male", "Angie", false },
                    { new Guid("b3f536a0-c4ef-49b2-9dbc-ac70a7bf21c2"), "97570 Raven Circle", new Guid("9fb9b388-f673-474e-8f61-189ac5ea07c7"), new DateTime(1988, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "mlingfoot5@netvibes.com", "Male", "Mitchael", false },
                    { new Guid("b8096b64-8369-4ab9-a60a-ff346a011823"), "9334 Fremont Street", new Guid("f79cb2c7-272c-4a67-8b60-a26deb03498d"), new DateTime(1987, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "vklussb@nationalgeographic.com", "Female", "Verene", false },
                    { new Guid("cd32113b-20d2-4783-95ef-0f713de26616"), "50467 Holy Cross Crossing", new Guid("9fb9b388-f673-474e-8f61-189ac5ea07c7"), new DateTime(1995, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "ttregona4@stumbleupon.com", "Gender", "Tani", false },
                    { new Guid("e72f5aeb-9b79-4edd-9888-22e3ed604a53"), "73 Heath Avenue", new Guid("3a8d6605-83d0-4158-9560-290829229a5a"), new DateTime(1995, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "fbowsher2@howstuffworks.com", "Male", "Franchot", false },
                    { new Guid("fdb709d0-8911-4424-9fca-aca38d69b455"), "4 Parkside Point", new Guid("d93c54b2-3d33-4854-a852-f0d2a43d4dde"), new DateTime(1989, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "mwebsdale0@people.com.cn", "Female", "Marguerite", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CountryId",
                table: "Persons",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
