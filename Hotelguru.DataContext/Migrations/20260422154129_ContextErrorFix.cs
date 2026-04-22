using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotelguru.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class ContextErrorFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationBenefits_Services_ServiceId",
                table: "ReservationBenefits");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "ReservationBenefits",
                newName: "BenefitId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationBenefits_ServiceId",
                table: "ReservationBenefits",
                newName: "IX_ReservationBenefits_BenefitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationBenefits_Services_BenefitId",
                table: "ReservationBenefits",
                column: "BenefitId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationBenefits_Services_BenefitId",
                table: "ReservationBenefits");

            migrationBuilder.RenameColumn(
                name: "BenefitId",
                table: "ReservationBenefits",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationBenefits_BenefitId",
                table: "ReservationBenefits",
                newName: "IX_ReservationBenefits_ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationBenefits_Services_ServiceId",
                table: "ReservationBenefits",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
