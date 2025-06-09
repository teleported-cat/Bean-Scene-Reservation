using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bean_Scene_Reservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixesReservationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_SittingTypes_SittingTypeId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_SittingTypeId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Date_SittingTypeId",
                table: "Reservations",
                columns: new[] { "Date", "SittingTypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Sittings_Date_SittingTypeId",
                table: "Reservations",
                columns: new[] { "Date", "SittingTypeId" },
                principalTable: "Sittings",
                principalColumns: new[] { "Date", "SittingTypeId" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Sittings_Date_SittingTypeId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_Date_SittingTypeId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SittingTypeId",
                table: "Reservations",
                column: "SittingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_SittingTypes_SittingTypeId",
                table: "Reservations",
                column: "SittingTypeId",
                principalTable: "SittingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
