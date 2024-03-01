using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class EmpiricalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmpiricalModelTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Alias = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpiricalModelTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParamsRangesUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Alias = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParamsRangesUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpiricalModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    Furmula = table.Column<string>(type: "TEXT", nullable: false),
                    EquipmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpiricalModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpiricalModels_EmpiricalModelTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "EmpiricalModelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpiricalModels_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpiricalModels_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpiricalModelCoeffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmpiricalModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Alias = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpiricalModelCoeffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpiricalModelCoeffs_EmpiricalModels_EmpiricalModelId",
                        column: x => x.EmpiricalModelId,
                        principalTable: "EmpiricalModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParamsRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxValue = table.Column<double>(type: "REAL", nullable: false),
                    MinValue = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParamsRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParamsRanges_EmpiricalModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "EmpiricalModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParamsRanges_ParamsRangesUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ParamsRangesUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpiricalModelCoeffs_EmpiricalModelId",
                table: "EmpiricalModelCoeffs",
                column: "EmpiricalModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpiricalModels_EquipmentId",
                table: "EmpiricalModels",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpiricalModels_MaterialId",
                table: "EmpiricalModels",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpiricalModels_TypeId",
                table: "EmpiricalModels",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ParamsRanges_ModelId",
                table: "ParamsRanges",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ParamsRanges_UnitId",
                table: "ParamsRanges",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpiricalModelCoeffs");

            migrationBuilder.DropTable(
                name: "ParamsRanges");

            migrationBuilder.DropTable(
                name: "EmpiricalModels");

            migrationBuilder.DropTable(
                name: "ParamsRangesUnits");

            migrationBuilder.DropTable(
                name: "EmpiricalModelTypes");
        }
    }
}
