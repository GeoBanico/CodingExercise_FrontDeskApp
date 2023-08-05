using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Db_FrontDeskApp.Migrations
{
    /// <inheritdoc />
    public partial class updatedCustomerRetrieveDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MeduimCapacity",
                table: "Facilities",
                newName: "MediumCapacity");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RetrievedDate",
                table: "Customers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediumCapacity",
                table: "Facilities",
                newName: "MeduimCapacity");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RetrievedDate",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
