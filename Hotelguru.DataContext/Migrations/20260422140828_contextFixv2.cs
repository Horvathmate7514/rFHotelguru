using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotelguru.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class contextFixv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Room_RoomId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_RoomTypes_RoomTypeId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomFacility_Facilities_FacilityId",
                table: "RoomFacility");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomFacility_Room_RoomId",
                table: "RoomFacility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomFacility",
                table: "RoomFacility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.RenameTable(
                name: "RoomFacility",
                newName: "RoomFacilities");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameIndex(
                name: "IX_RoomFacility_RoomId",
                table: "RoomFacilities",
                newName: "IX_RoomFacilities_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomFacility_FacilityId",
                table: "RoomFacilities",
                newName: "IX_RoomFacilities_FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_RoomTypeId",
                table: "Rooms",
                newName: "IX_Rooms_RoomTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomFacilities",
                table: "RoomFacilities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Rooms_RoomId",
                table: "Reservations",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFacilities_Facilities_FacilityId",
                table: "RoomFacilities",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFacilities_Rooms_RoomId",
                table: "RoomFacilities",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeId",
                table: "Rooms",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Rooms_RoomId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomFacilities_Facilities_FacilityId",
                table: "RoomFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomFacilities_Rooms_RoomId",
                table: "RoomFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomFacilities",
                table: "RoomFacilities");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameTable(
                name: "RoomFacilities",
                newName: "RoomFacility");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Room",
                newName: "IX_Room_RoomTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomFacilities_RoomId",
                table: "RoomFacility",
                newName: "IX_RoomFacility_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomFacilities_FacilityId",
                table: "RoomFacility",
                newName: "IX_RoomFacility_FacilityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomFacility",
                table: "RoomFacility",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Room_RoomId",
                table: "Reservations",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_RoomTypes_RoomTypeId",
                table: "Room",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFacility_Facilities_FacilityId",
                table: "RoomFacility",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFacility_Room_RoomId",
                table: "RoomFacility",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
