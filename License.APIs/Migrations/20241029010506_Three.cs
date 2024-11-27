using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace License.APIs.Migrations
{
    /// <inheritdoc />
    public partial class Three : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Certificates",
                newName: "Active");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Certificates",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Certificates",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivationDate",
                table: "Certificates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CertificateType",
                table: "Certificates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "Certificates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationDate",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "CertificateType",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Certificates",
                newName: "IsActive");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Certificates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Certificates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Certificates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
