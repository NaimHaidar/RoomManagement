using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomManagement.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteAttendees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Attendee__Meetin__3D7E1B63",
                table: "Attendee");

            migrationBuilder.AddForeignKey(
                name: "FK__Attendee__Meetin__3D7E1B63",
                table: "Attendee",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Attendee__Meetin__3D7E1B63",
                table: "Attendee");

            migrationBuilder.AddForeignKey(
                name: "FK__Attendee__Meetin__3D7E1B63",
                table: "Attendee",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id");
        }
    }
}
