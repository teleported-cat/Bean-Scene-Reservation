using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bean_Scene_Reservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForAreaAndTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Main" },
                    { 2, "Outside" },
                    { 3, "Balcony" }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "TableNumber", "AreaId" },
                values: new object[,]
                {
                    { "B1", 3 },
                    { "B10", 3 },
                    { "B2", 3 },
                    { "B3", 3 },
                    { "B4", 3 },
                    { "B5", 3 },
                    { "B6", 3 },
                    { "B7", 3 },
                    { "B8", 3 },
                    { "B9", 3 },
                    { "M1", 1 },
                    { "M10", 1 },
                    { "M2", 1 },
                    { "M3", 1 },
                    { "M4", 1 },
                    { "M5", 1 },
                    { "M6", 1 },
                    { "M7", 1 },
                    { "M8", 1 },
                    { "M9", 1 },
                    { "O1", 2 },
                    { "O10", 2 },
                    { "O2", 2 },
                    { "O3", 2 },
                    { "O4", 2 },
                    { "O5", 2 },
                    { "O6", 2 },
                    { "O7", 2 },
                    { "O8", 2 },
                    { "O9", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "B1");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "B10");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "B2");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "B3");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "B4");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "B5");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "B6");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "B7");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "B8");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "B9");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "M1");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "M10");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "M2");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "M3");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "M4");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "M5");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "M6");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "M7");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "M8");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "M9");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "O1");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "O10");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "O2");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "O3");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "O4");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "O5");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "O6");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "O7");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "O8");

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableNumber",
                keyValue: "O9");

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
