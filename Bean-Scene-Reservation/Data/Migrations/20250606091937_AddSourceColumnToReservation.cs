using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bean_Scene_Reservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSourceColumnToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Reservations");
        }
    }
}
