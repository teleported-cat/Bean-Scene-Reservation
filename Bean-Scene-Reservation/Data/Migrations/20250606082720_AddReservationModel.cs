using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bean_Scene_Reservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    SittingTypeId = table.Column<int>(type: "int", nullable: false),
                    StartTimeId = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTimeId = table.Column<TimeOnly>(type: "time", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Sittings_Date_SittingTypeId",
                        columns: x => new { x.Date, x.SittingTypeId },
                        principalTable: "Sittings",
                        principalColumns: new[] { "Date", "SittingTypeId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Timeslots_EndTimeId",
                        column: x => x.EndTimeId,
                        principalTable: "Timeslots",
                        principalColumn: "Time",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Timeslots_StartTimeId",
                        column: x => x.StartTimeId,
                        principalTable: "Timeslots",
                        principalColumn: "Time",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservationTable",
                columns: table => new
                {
                    ReservationsId = table.Column<int>(type: "int", nullable: false),
                    TableNumber = table.Column<string>(type: "nvarchar(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationTable", x => new { x.ReservationsId, x.TableNumber });
                    table.ForeignKey(
                        name: "FK_ReservationTable_Reservations_ReservationsId",
                        column: x => x.ReservationsId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationTable_Tables_TableNumber",
                        column: x => x.TableNumber,
                        principalTable: "Tables",
                        principalColumn: "TableNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AreaId",
                table: "Reservations",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Date_SittingTypeId",
                table: "Reservations",
                columns: new[] { "Date", "SittingTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_EndTimeId",
                table: "Reservations",
                column: "EndTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_StartTimeId",
                table: "Reservations",
                column: "StartTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationTable_TableNumber",
                table: "ReservationTable",
                column: "TableNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationTable");

            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
