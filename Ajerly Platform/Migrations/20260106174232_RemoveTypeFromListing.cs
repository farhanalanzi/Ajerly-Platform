using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ajerly_Platform.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTypeFromListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Listings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
