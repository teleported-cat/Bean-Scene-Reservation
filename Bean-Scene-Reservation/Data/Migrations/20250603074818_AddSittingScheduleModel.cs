using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bean_Scene_Reservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSittingScheduleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SittingScheduleId",
                table: "Sittings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SittingSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTimeId = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTimeId = table.Column<TimeOnly>(type: "time", nullable: false),
                    SittingTypeId = table.Column<int>(type: "int", nullable: false),
                    ForMonday = table.Column<bool>(type: "bit", nullable: false),
                    ForTuesday = table.Column<bool>(type: "bit", nullable: false),
                    ForWednesday = table.Column<bool>(type: "bit", nullable: false),
                    ForThursday = table.Column<bool>(type: "bit", nullable: false),
                    ForFriday = table.Column<bool>(type: "bit", nullable: false),
                    ForSaturday = table.Column<bool>(type: "bit", nullable: false),
                    ForSunday = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SittingSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SittingSchedules_SittingTypes_SittingTypeId",
                        column: x => x.SittingTypeId,
                        principalTable: "SittingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SittingSchedules_Timeslots_EndTimeId",
                        column: x => x.EndTimeId,
                        principalTable: "Timeslots",
                        principalColumn: "Time",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SittingSchedules_Timeslots_StartTimeId",
                        column: x => x.StartTimeId,
                        principalTable: "Timeslots",
                        principalColumn: "Time",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sittings_SittingScheduleId",
                table: "Sittings",
                column: "SittingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SittingSchedules_EndTimeId",
                table: "SittingSchedules",
                column: "EndTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_SittingSchedules_SittingTypeId",
                table: "SittingSchedules",
                column: "SittingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SittingSchedules_StartTimeId",
                table: "SittingSchedules",
                column: "StartTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sittings_SittingSchedules_SittingScheduleId",
                table: "Sittings",
                column: "SittingScheduleId",
                principalTable: "SittingSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sittings_SittingSchedules_SittingScheduleId",
                table: "Sittings");

            migrationBuilder.DropTable(
                name: "SittingSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Sittings_SittingScheduleId",
                table: "Sittings");

            migrationBuilder.DropColumn(
                name: "SittingScheduleId",
                table: "Sittings");
        }
    }
}
