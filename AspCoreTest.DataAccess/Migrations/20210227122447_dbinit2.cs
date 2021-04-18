using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCoreTest.DataAccess.Migrations
{
    public partial class dbinit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "ROLES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "UUID()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "PERSONS",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "UUID()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "PERSON_ROLES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "UUID()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "PERSON_CVS",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "UUID()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "PERSON_ADDRESSES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "UUID()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "COUNTRIES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "UUID()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "CITIES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "UUID()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "ADMINS",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "UUID()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "ADDRESSES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "UUID()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "newid()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "ROLES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "UUID()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "PERSONS",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "UUID()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "PERSON_ROLES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "UUID()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "PERSON_CVS",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "UUID()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "PERSON_ADDRESSES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "UUID()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "COUNTRIES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "UUID()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "CITIES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "UUID()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "ADMINS",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "UUID()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "ADDRESSES",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "UUID()");
        }
    }
}
