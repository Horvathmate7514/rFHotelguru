using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotelguru.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class BenefitRenamesV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationBenefits_Services_ServiceId",
                table: "ReservationBenefits");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "ReservationBenefits",
                newName: "BenefitId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationBenefits_ServiceId",
                table: "ReservationBenefits",
                newName: "IX_ReservationBenefits_BenefitId");

            migrationBuilder.CreateTable(
                name: "Benefits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefits", x => x.Id);
                });

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

            migrationBuilder.DropTable(
                name: "Benefits");

            migrationBuilder.RenameColumn(
                name: "BenefitId",
                table: "ReservationBenefits",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationBenefits_BenefitId",
                table: "ReservationBenefits",
                newName: "IX_ReservationBenefits_ServiceId");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

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
