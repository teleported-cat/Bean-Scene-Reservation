using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bean_Scene_Reservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForTimeslot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Timeslots",
                column: "Time",
                values: new object[]
                {
                    new TimeOnly(8, 0, 0),
                    new TimeOnly(8, 30, 0),
                    new TimeOnly(9, 0, 0),
                    new TimeOnly(9, 30, 0),
                    new TimeOnly(10, 0, 0),
                    new TimeOnly(10, 30, 0),
                    new TimeOnly(11, 0, 0),
                    new TimeOnly(11, 30, 0),
                    new TimeOnly(12, 0, 0),
                    new TimeOnly(12, 30, 0),
                    new TimeOnly(13, 0, 0),
                    new TimeOnly(13, 30, 0),
                    new TimeOnly(14, 0, 0),
                    new TimeOnly(14, 30, 0),
                    new TimeOnly(15, 0, 0),
                    new TimeOnly(15, 30, 0),
                    new TimeOnly(16, 0, 0),
                    new TimeOnly(16, 30, 0),
                    new TimeOnly(17, 0, 0),
                    new TimeOnly(17, 30, 0),
                    new TimeOnly(18, 0, 0),
                    new TimeOnly(18, 30, 0),
                    new TimeOnly(19, 0, 0),
                    new TimeOnly(19, 30, 0),
                    new TimeOnly(20, 0, 0),
                    new TimeOnly(20, 30, 0),
                    new TimeOnly(21, 0, 0),
                    new TimeOnly(21, 30, 0),
                    new TimeOnly(22, 0, 0)
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(8, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(8, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(9, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(9, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(10, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(10, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(11, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(11, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(12, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(12, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(13, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(13, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(14, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(14, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(15, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(15, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(16, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(16, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(17, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(17, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(18, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(18, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(19, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(19, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(20, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(20, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(21, 0, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(21, 30, 0));

            migrationBuilder.DeleteData(
                table: "Timeslots",
                keyColumn: "Time",
                keyValue: new TimeOnly(22, 0, 0));
        }
    }
}
