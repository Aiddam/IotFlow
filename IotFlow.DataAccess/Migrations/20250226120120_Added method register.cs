using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IotFlow.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Addedmethodregister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "MethodRegister",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MethodId = table.Column<int>(type: "integer", nullable: false),
                    MethodName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResponseReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Result = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DeviceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodRegister", x => x.Id);
                    table.UniqueConstraint("AK_MethodRegister_CorrelationId", x => x.CorrelationId);
                    table.ForeignKey(
                        name: "FK_MethodRegister_DevicesMethods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "DevicesMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodRegister_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodRegister_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MethodRegister_DeviceId",
                table: "MethodRegister",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodRegister_MethodId",
                table: "MethodRegister",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodRegister_UserId",
                table: "MethodRegister",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MethodRegister");
        }
    }
}
