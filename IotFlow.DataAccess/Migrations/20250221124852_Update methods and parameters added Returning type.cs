using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IotFlow.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatemethodsandparametersaddedReturningtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MethodType",
                table: "DevicesMethods",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MethodType",
                table: "DevicesMethods");
        }
    }
}
