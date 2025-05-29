using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bean_Scene_Reservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintForAreaName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Areas_Name",
                table: "Areas",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Areas_Name",
                table: "Areas");
        }
    }
}
