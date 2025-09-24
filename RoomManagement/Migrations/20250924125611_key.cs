using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomManagement.Migrations
{
    /// <inheritdoc />
    public partial class key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Meeting__RoomId__33008CF0",
                table: "Meeting");

            migrationBuilder.AddForeignKey(
                name: "FK__Meeting__RoomId__33008CF0",
                table: "Meeting",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Meeting__RoomId__33008CF0",
                table: "Meeting");

            migrationBuilder.AddForeignKey(
                name: "FK__Meeting__RoomId__33008CF0",
                table: "Meeting",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id");
        }
    }
}
