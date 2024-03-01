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
                name: "EmpiricalModelType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Alias = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpiricalModelType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParamRangeUnit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Alias = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParamRangeUnit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpiricalModel",
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
                    table.PrimaryKey("PK_EmpiricalModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpiricalModel_EmpiricalModelType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "EmpiricalModelType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpiricalModel_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpiricalModel_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpiricalModelCoeff",
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
                    table.PrimaryKey("PK_EmpiricalModelCoeff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpiricalModelCoeff_EmpiricalModel_EmpiricalModelId",
                        column: x => x.EmpiricalModelId,
                        principalTable: "EmpiricalModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParamRange",
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
                    table.PrimaryKey("PK_ParamRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParamRange_EmpiricalModel_ModelId",
                        column: x => x.ModelId,
                        principalTable: "EmpiricalModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParamRange_ParamRangeUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ParamRangeUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpiricalModel_EquipmentId",
                table: "EmpiricalModel",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpiricalModel_MaterialId",
                table: "EmpiricalModel",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpiricalModel_TypeId",
                table: "EmpiricalModel",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpiricalModelCoeff_EmpiricalModelId",
                table: "EmpiricalModelCoeff",
                column: "EmpiricalModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ParamRange_ModelId",
                table: "ParamRange",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ParamRange_UnitId",
                table: "ParamRange",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpiricalModelCoeff");

            migrationBuilder.DropTable(
                name: "ParamRange");

            migrationBuilder.DropTable(
                name: "EmpiricalModel");

            migrationBuilder.DropTable(
                name: "ParamRangeUnit");

            migrationBuilder.DropTable(
                name: "EmpiricalModelType");
        }
    }
}
