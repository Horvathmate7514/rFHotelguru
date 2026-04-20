using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotelguru.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomToFacility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_Rooms_RoomId",
                table: "Facilities");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Facilities",
                newName: "RoomID");

            migrationBuilder.RenameIndex(
                name: "IX_Facilities_RoomId",
                table: "Facilities",
                newName: "IX_Facilities_RoomID");

            migrationBuilder.AlterColumn<int>(
                name: "RoomID",
                table: "Facilities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_Rooms_RoomID",
                table: "Facilities",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_Rooms_RoomID",
                table: "Facilities");

            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "Facilities",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Facilities_RoomID",
                table: "Facilities",
                newName: "IX_Facilities_RoomId");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Facilities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_Rooms_RoomId",
                table: "Facilities",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
