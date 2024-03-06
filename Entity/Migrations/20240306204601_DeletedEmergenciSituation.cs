using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class DeletedEmergenciSituation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmergencySituationTask");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_EmergencySituationId",
                table: "Tasks",
                column: "EmergencySituationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_EmergencySituations_EmergencySituationId",
                table: "Tasks",
                column: "EmergencySituationId",
                principalTable: "EmergencySituations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_EmergencySituations_EmergencySituationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_EmergencySituationId",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "EmergencySituationTask",
                columns: table => new
                {
                    EmergencySituationId = table.Column<int>(type: "INTEGER", nullable: false),
                    TasksId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencySituationTask", x => new { x.EmergencySituationId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_EmergencySituationTask_EmergencySituations_EmergencySituationId",
                        column: x => x.EmergencySituationId,
                        principalTable: "EmergencySituations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmergencySituationTask_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmergencySituationTask_TasksId",
                table: "EmergencySituationTask",
                column: "TasksId");
        }
    }
}
