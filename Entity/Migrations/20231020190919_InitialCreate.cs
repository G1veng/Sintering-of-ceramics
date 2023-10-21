using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ControlActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Unit = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentType = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentBrand = table.Column<string>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    ChargeWightLoad = table.Column<double>(type: "REAL", nullable: false),
                    HeaterType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaterialType = table.Column<int>(type: "INTEGER", nullable: false),
                    Porosity = table.Column<double>(type: "REAL", nullable: false),
                    AvarageGrainSize = table.Column<double>(type: "REAL", nullable: false),
                    SurfaceLayerThickness = table.Column<double>(type: "REAL", nullable: false),
                    SpecificSurfaceEnergy = table.Column<double>(type: "REAL", nullable: false),
                    CompactMaterialDensity = table.Column<double>(type: "REAL", nullable: false),
                    CompactMaterialViscosity = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MMs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fisher = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MMs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Qualities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Unit = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MinSinteringTime = table.Column<double>(type: "REAL", nullable: false),
                    MaxSinteringTime = table.Column<double>(type: "REAL", nullable: false),
                    MinFinalTempretare = table.Column<double>(type: "REAL", nullable: false),
                    MaxFinalTempretare = table.Column<double>(type: "REAL", nullable: false),
                    MinCuringTime = table.Column<double>(type: "REAL", nullable: false),
                    MaxCuringTime = table.Column<double>(type: "REAL", nullable: false),
                    MinGasPressure = table.Column<double>(type: "REAL", nullable: false),
                    MaxGasPressure = table.Column<double>(type: "REAL", nullable: false),
                    EquipmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regimes_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TheoreticalMMParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PreExponentialFactorOfGraindBoundaryDiffusionCoefficient = table.Column<double>(type: "REAL", nullable: false),
                    PreExponentialFactorOfSurfaceSelfCoefficient = table.Column<double>(type: "REAL", nullable: false),
                    GrainBoundaryDiffusionActivationEnergy = table.Column<double>(type: "REAL", nullable: false),
                    SurfaceSelfDiffusionActivationEnergy = table.Column<double>(type: "REAL", nullable: false),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheoreticalMMParams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TheoreticalMMParams_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MMCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    MMId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MMCoefficients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MMCoefficients_MMs_MMId",
                        column: x => x.MMId,
                        principalTable: "MMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentalDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstControlActionId = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondControlActionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ThirdControlActionId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstActionValue = table.Column<double>(type: "REAL", nullable: false),
                    SecondActionValue = table.Column<double>(type: "REAL", nullable: false),
                    ThirdActionValue = table.Column<double>(type: "REAL", nullable: false),
                    QualitiesValue = table.Column<double>(type: "REAL", nullable: false),
                    QualitiesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentalDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentalDatas_ControlActions_FirstControlActionId",
                        column: x => x.FirstControlActionId,
                        principalTable: "ControlActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentalDatas_ControlActions_SecondControlActionId",
                        column: x => x.SecondControlActionId,
                        principalTable: "ControlActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentalDatas_ControlActions_ThirdControlActionId",
                        column: x => x.ThirdControlActionId,
                        principalTable: "ControlActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentalDatas_Qualities_QualitiesId",
                        column: x => x.QualitiesId,
                        principalTable: "Qualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MMId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    QualityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Models_MMs_MMId",
                        column: x => x.MMId,
                        principalTable: "MMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Models_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Models_Qualities_QualityId",
                        column: x => x.QualityId,
                        principalTable: "Qualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentalDatas_FirstControlActionId",
                table: "ExperimentalDatas",
                column: "FirstControlActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentalDatas_QualitiesId",
                table: "ExperimentalDatas",
                column: "QualitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentalDatas_SecondControlActionId",
                table: "ExperimentalDatas",
                column: "SecondControlActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentalDatas_ThirdControlActionId",
                table: "ExperimentalDatas",
                column: "ThirdControlActionId");

            migrationBuilder.CreateIndex(
                name: "IX_MMCoefficients_MMId",
                table: "MMCoefficients",
                column: "MMId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_EquipmentId",
                table: "Models",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_MaterialId",
                table: "Models",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_MMId",
                table: "Models",
                column: "MMId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_QualityId",
                table: "Models",
                column: "QualityId");

            migrationBuilder.CreateIndex(
                name: "IX_Regimes_EquipmentId",
                table: "Regimes",
                column: "EquipmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TheoreticalMMParams_MaterialId",
                table: "TheoreticalMMParams",
                column: "MaterialId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperimentalDatas");

            migrationBuilder.DropTable(
                name: "MMCoefficients");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Regimes");

            migrationBuilder.DropTable(
                name: "TheoreticalMMParams");

            migrationBuilder.DropTable(
                name: "ControlActions");

            migrationBuilder.DropTable(
                name: "MMs");

            migrationBuilder.DropTable(
                name: "Qualities");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Materials");
        }
    }
}
