using Microsoft.EntityFrameworkCore.Migrations;

namespace Entity.Migrations.PostDeploymentScripts
{
    internal class AddMaterials
    {
        public static void AddUpScripts(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP TABLE IF EXISTS tempMaterials;
                CREATE TEMP TABLE tempMaterials AS SELECT * FROM Materials;
                INSERT INTO tempMaterials (Id, MaterialType, Porosity, AvarageGrainSize, SurfaceLayerThickness,
                    SpecificSurfaceEnergy, CompactMaterialDensity, CompactMaterialViscosity, Weight, Name) VALUES 
                (1, 1, 40, 1, 0.1, 3.5, 14600, 170, 0.1, 'Вольфрам кобальт'),
                (2, 2, 20, 0.5, 0.5, 5, 12500, 50, 0.1, 'Вольфрам никель'),
                (3, 3, 60, 2, 0.2, 4, 10300, 70, 0.1, 'Диоксид циркония'),
                (4, 4, 70, 0.3, 0.5, 6, 13500, 40, 0.1, 'Оксид иттрия'),
                (5, 5, 5, 1.5, 1, 10, 15000, 10, 0.1, 'Оксид алюминия');
                INSERT INTO Materials SELECT * FROM tempMaterials
                WHERE NOT EXISTS (SELECT * FROM Materials WHERE tempMaterials.Id = Materials.Id);
                UPDATE Materials SET MaterialType = (SELECT MaterialType FROM tempMaterials WHERE tempMaterials.Id = Materials.Id);
                UPDATE Materials SET Porosity = (SELECT Porosity FROM tempMaterials WHERE tempMaterials.Id = Materials.Id);
                UPDATE Materials SET AvarageGrainSize = (SELECT AvarageGrainSize FROM tempMaterials WHERE tempMaterials.Id = Materials.Id);
                UPDATE Materials SET SurfaceLayerThickness = (SELECT SurfaceLayerThickness FROM tempMaterials WHERE tempMaterials.Id = Materials.Id);
                UPDATE Materials SET SpecificSurfaceEnergy = (SELECT SpecificSurfaceEnergy FROM tempMaterials WHERE tempMaterials.Id = Materials.Id);
                UPDATE Materials SET CompactMaterialDensity = (SELECT CompactMaterialDensity FROM tempMaterials WHERE tempMaterials.Id = Materials.Id);
                UPDATE Materials SET CompactMaterialViscosity = (SELECT CompactMaterialViscosity FROM tempMaterials WHERE tempMaterials.Id = Materials.Id);

                DROP TABLE IF EXISTS tempTheoreticalMMParams;
                CREATE TEMP TABLE tempTheoreticalMMParams AS SELECT * FROM TheoreticalMMParams;
                INSERT INTO tempTheoreticalMMParams (Id, PreExponentialFactorOfGraindBoundaryDiffusionCoefficient,
                    PreExponentialFactorOfSurfaceSelfCoefficient, GrainBoundaryDiffusionActivationEnergy,
                    SurfaceSelfDiffusionActivationEnergy, MaterialId) VALUES 
                (1, 0.35, 0.4, 176, 245, 1),
                (2, 0.25, 0.5, 260, 300, 2),
                (3, 0.2, 0.3, 152.5, 200, 3),
                (4, 0.55, 0.55, 198.5, 220, 4),
                (5, 0.15, 0.2, 177, 200, 5);
                INSERT INTO TheoreticalMMParams SELECT * FROM tempTheoreticalMMParams
                WHERE NOT EXISTS (SELECT * FROM TheoreticalMMParams WHERE tempTheoreticalMMParams.Id = TheoreticalMMParams.Id);
                UPDATE TheoreticalMMParams SET PreExponentialFactorOfGraindBoundaryDiffusionCoefficient = (SELECT PreExponentialFactorOfGraindBoundaryDiffusionCoefficient FROM tempTheoreticalMMParams WHERE tempTheoreticalMMParams.Id = TheoreticalMMParams.Id);
                UPDATE TheoreticalMMParams SET PreExponentialFactorOfSurfaceSelfCoefficient = (SELECT PreExponentialFactorOfSurfaceSelfCoefficient FROM tempTheoreticalMMParams WHERE tempTheoreticalMMParams.Id = TheoreticalMMParams.Id);
                UPDATE TheoreticalMMParams SET GrainBoundaryDiffusionActivationEnergy = (SELECT GrainBoundaryDiffusionActivationEnergy FROM tempTheoreticalMMParams WHERE tempTheoreticalMMParams.Id = TheoreticalMMParams.Id);
                UPDATE TheoreticalMMParams SET SurfaceSelfDiffusionActivationEnergy = (SELECT SurfaceSelfDiffusionActivationEnergy FROM tempTheoreticalMMParams WHERE tempTheoreticalMMParams.Id = TheoreticalMMParams.Id);");
        }
    }
}
