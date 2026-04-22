using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotelguru.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class ContextErrorFixV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationBenefits_Services_BenefitId",
                table: "ReservationBenefits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "Benefits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Benefits",
                table: "Benefits",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationBenefits_Benefits_BenefitId",
                table: "ReservationBenefits",
                column: "BenefitId",
                principalTable: "Benefits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationBenefits_Benefits_BenefitId",
                table: "ReservationBenefits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Benefits",
                table: "Benefits");

            migrationBuilder.RenameTable(
                name: "Benefits",
                newName: "Services");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationBenefits_Services_BenefitId",
                table: "ReservationBenefits",
                column: "BenefitId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
