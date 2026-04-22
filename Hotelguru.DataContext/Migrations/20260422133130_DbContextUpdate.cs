using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotelguru.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class DbContextUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Rooms_RoomId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationServices_Reservations_ReservationId",
                table: "ReservationServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationServices_Services_ServiceId",
                table: "ReservationServices");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomFacility_Rooms_RoomId",
                table: "RoomFacility");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationServices",
                table: "ReservationServices");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameTable(
                name: "ReservationServices",
                newName: "ReservationBenefits");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Room",
                newName: "IX_Room_RoomTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationServices_ServiceId",
                table: "ReservationBenefits",
                newName: "IX_ReservationBenefits_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationServices_ReservationId",
                table: "ReservationBenefits",
                newName: "IX_ReservationBenefits_ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationBenefits",
                table: "ReservationBenefits",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationBenefits_Reservations_ReservationId",
                table: "ReservationBenefits",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationBenefits_Services_ServiceId",
                table: "ReservationBenefits",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_RoomFacility_Room_RoomId",
                table: "RoomFacility",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationBenefits_Reservations_ReservationId",
                table: "ReservationBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationBenefits_Services_ServiceId",
                table: "ReservationBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Room_RoomId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_RoomTypes_RoomTypeId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomFacility_Room_RoomId",
                table: "RoomFacility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationBenefits",
                table: "ReservationBenefits");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "ReservationBenefits",
                newName: "ReservationServices");

            migrationBuilder.RenameIndex(
                name: "IX_Room_RoomTypeId",
                table: "Rooms",
                newName: "IX_Rooms_RoomTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationBenefits_ServiceId",
                table: "ReservationServices",
                newName: "IX_ReservationServices_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationBenefits_ReservationId",
                table: "ReservationServices",
                newName: "IX_ReservationServices_ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationServices",
                table: "ReservationServices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Rooms_RoomId",
                table: "Reservations",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationServices_Reservations_ReservationId",
                table: "ReservationServices",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationServices_Services_ServiceId",
                table: "ReservationServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFacility_Rooms_RoomId",
                table: "RoomFacility",
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
    }
}
