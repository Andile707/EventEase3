using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventease.Migrations
{
    /// <inheritdoc />
    public partial class VenueImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.AddColumn<string>(
                name: "venueImage",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropColumn(
                name: "venueImage",
                table: "Venues");*/
        }
    }
}
