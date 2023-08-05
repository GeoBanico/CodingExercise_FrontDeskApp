using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Db_FrontDeskApp.Migrations
{
    /// <inheritdoc />
    public partial class remainingCapacityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LargeRemainingCapacity",
                table: "Facilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MediumRemainingCapacity",
                table: "Facilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SmallRemainingCapacity",
                table: "Facilities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LargeRemainingCapacity",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "MediumRemainingCapacity",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "SmallRemainingCapacity",
                table: "Facilities");
        }
    }
}
