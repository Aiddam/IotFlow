using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IotFlow.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Addeddescriptiontodevicesparamsandmethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DevicesMethods",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DeviceMethodParameters",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DevicesMethods");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DeviceMethodParameters");
        }
    }
}
