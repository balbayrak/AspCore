using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCoreTest.DataAccess.Migrations
{
    public partial class dbinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspCoreAutoHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActiveUserID = table.Column<Guid>(type: "char(36)", nullable: false),
                    RowId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TableName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Changed = table.Column<string>(type: "longtext", maxLength: 5000, nullable: true),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspCoreAutoHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COUNTRIES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    NAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    LAST_UPDATE_TIME = table.Column<DateTime>(type: "datetime", nullable: false),
                    IS_DELETED = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    LAST_UPDATED_USERID = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COUNTRIES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PERSONS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    NAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SURNAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    LAST_UPDATE_TIME = table.Column<DateTime>(type: "datetime", nullable: false),
                    IS_DELETED = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    LAST_UPDATED_USERID = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSONS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    NAME = table.Column<Guid>(type: "char(36)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    LAST_UPDATE_TIME = table.Column<DateTime>(type: "datetime", nullable: false),
                    IS_DELETED = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    LAST_UPDATED_USERID = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CITIES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    NAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    COUNTRY_ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    LAST_UPDATE_TIME = table.Column<DateTime>(type: "datetime", nullable: false),
                    IS_DELETED = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    LAST_UPDATED_USERID = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITIES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CITY_COUNTRY",
                        column: x => x.COUNTRY_ID,
                        principalTable: "COUNTRIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ADMINS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMINS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PERSON_ADMIN",
                        column: x => x.ID,
                        principalTable: "PERSONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PERSON_CVS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    NAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DOCUMENT_URL = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PERSON_ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    LAST_UPDATE_TIME = table.Column<DateTime>(type: "datetime", nullable: false),
                    IS_DELETED = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    LAST_UPDATED_USERID = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSON_CVS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PERSON_CV_PERSON",
                        column: x => x.PERSON_ID,
                        principalTable: "PERSONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PERSON_ROLES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    ROLE_ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    PERSON_ID = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSON_ROLES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PERSON_ROLE_PERSON",
                        column: x => x.PERSON_ID,
                        principalTable: "PERSONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PERSON_ROLE_ROLE",
                        column: x => x.ROLE_ID,
                        principalTable: "ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ADDRESSES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    FULL_ADDRESS = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    CITY_ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    LAST_UPDATE_TIME = table.Column<DateTime>(type: "datetime", nullable: false),
                    IS_DELETED = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    LAST_UPDATED_USERID = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADDRESSES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ADDRESS_CITY",
                        column: x => x.CITY_ID,
                        principalTable: "CITIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PERSON_ADDRESSES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    PERSON_ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    ADDRESS_ID = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSON_ADDRESSES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PERSON_ADDRESS_ADDRESS",
                        column: x => x.ADDRESS_ID,
                        principalTable: "ADDRESSES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PERSON_ADDRESS_PERSON",
                        column: x => x.PERSON_ID,
                        principalTable: "PERSONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADDRESSES_CITY_ID",
                table: "ADDRESSES",
                column: "CITY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CITIES_COUNTRY_ID",
                table: "CITIES",
                column: "COUNTRY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PERSON_ADDRESSES_ADDRESS_ID",
                table: "PERSON_ADDRESSES",
                column: "ADDRESS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PERSON_ADDRESSES_PERSON_ID",
                table: "PERSON_ADDRESSES",
                column: "PERSON_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PERSON_CVS_PERSON_ID",
                table: "PERSON_CVS",
                column: "PERSON_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PERSON_ROLES_PERSON_ID",
                table: "PERSON_ROLES",
                column: "PERSON_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PERSON_ROLES_ROLE_ID",
                table: "PERSON_ROLES",
                column: "ROLE_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADMINS");

            migrationBuilder.DropTable(
                name: "AspCoreAutoHistory");

            migrationBuilder.DropTable(
                name: "PERSON_ADDRESSES");

            migrationBuilder.DropTable(
                name: "PERSON_CVS");

            migrationBuilder.DropTable(
                name: "PERSON_ROLES");

            migrationBuilder.DropTable(
                name: "ADDRESSES");

            migrationBuilder.DropTable(
                name: "PERSONS");

            migrationBuilder.DropTable(
                name: "ROLES");

            migrationBuilder.DropTable(
                name: "CITIES");

            migrationBuilder.DropTable(
                name: "COUNTRIES");
        }
    }
}
