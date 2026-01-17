using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ajerly_Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeToListingsAndRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Listings");
        }
    }
}
