using Entity.Migrations.PostDeploymentScripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class RoleEmergencyScriptTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmergencySituations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Reason = table.Column<string>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencySituations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Alias = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scripts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Protocol = table.Column<string>(type: "TEXT", nullable: false),
                    TaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    InstructorId = table.Column<int>(type: "INTEGER", nullable: false),
                    TraineeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scripts_Users_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scripts_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmergencySituationId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScriptId = table.Column<int>(type: "INTEGER", nullable: false),
                    RegimeId = table.Column<int>(type: "INTEGER", nullable: false),
                    QualityId = table.Column<int>(type: "INTEGER", nullable: false),
                    OvenTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Equipments_OvenTypeId",
                        column: x => x.OvenTypeId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Qualities_QualityId",
                        column: x => x.QualityId,
                        principalTable: "Qualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Regimes_RegimeId",
                        column: x => x.RegimeId,
                        principalTable: "Regimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Scripts_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencySituationTask_TasksId",
                table: "EmergencySituationTask",
                column: "TasksId");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_InstructorId",
                table: "Scripts",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_TraineeId",
                table: "Scripts",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_MaterialId",
                table: "Tasks",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OvenTypeId",
                table: "Tasks",
                column: "OvenTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_QualityId",
                table: "Tasks",
                column: "QualityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_RegimeId",
                table: "Tasks",
                column: "RegimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ScriptId",
                table: "Tasks",
                column: "ScriptId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "EmergencySituationTask");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "EmergencySituations");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Scripts");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");
        }
    }
}
