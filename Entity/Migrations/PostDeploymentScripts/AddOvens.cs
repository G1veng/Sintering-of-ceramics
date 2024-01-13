using Microsoft.EntityFrameworkCore.Migrations;

namespace Entity.Migrations.PostDeploymentScripts
{
    internal class AddOvens
    {
        public static void AddUpScripts(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP TABLE IF EXISTS tempOvens;
                CREATE TEMP TABLE tempOvens AS SELECT * FROM Equipments;
                INSERT INTO tempOvens (Id, EquipmentType, EquipmentBrand, Manufacturer, ChargeWightLoad, HeaterType) VALUES 
                (1, 'Горизонтальная', 'COD 633R - 100', 'PVA-TePla GmbH, Германия', 0.0, 0),
                (2, 'Горизонтальная', 'COD 733 RL - 2000 - 100', 'PVA-TePla GmbH, Германия', 0.0, 0),
                (3, 'Вертикальная', 'FP W 90 - SD', 'FCT Systeme GmbH, Германия', 0.0, 0);
                INSERT INTO Equipments SELECT * FROM tempOvens
                WHERE NOT EXISTS (SELECT * FROM Equipments WHERE tempOvens.Id = Equipments.Id);
                UPDATE Equipments SET EquipmentType = (SELECT EquipmentType FROM tempOvens WHERE tempOvens.Id = Equipments.Id);
                UPDATE Equipments SET EquipmentBrand = (SELECT EquipmentBrand FROM tempOvens WHERE tempOvens.Id = Equipments.Id);
                UPDATE Equipments SET Manufacturer = (SELECT Manufacturer FROM tempOvens WHERE tempOvens.Id = Equipments.Id);
                UPDATE Equipments SET ChargeWightLoad = (SELECT ChargeWightLoad FROM tempOvens WHERE tempOvens.Id = Equipments.Id);
                UPDATE Equipments SET HeaterType = (SELECT HeaterType FROM tempOvens WHERE tempOvens.Id = Equipments.Id);

                DROP TABLE IF EXISTS tempRegimes;
                CREATE TEMP TABLE tempRegimes AS SELECT * FROM Regimes;
                INSERT INTO tempRegimes (Id, MinSinteringTime, MaxSinteringTime, MinFinalTempretare, MaxFinalTempretare, 
                                        MinCuringTime, MaxCuringTime, MinGasPressure, MaxGasPressure, EquipmentId) VALUES 
                (1, 50, 90, 1200, 1600, 20, 50, 4, 8, 1),
                (2, 50, 90, 1200, 1600, 20, 50, 4, 8, 2),
                (3, 50, 90, 1200, 1600, 20, 50, 4, 8, 3);
                INSERT INTO Regimes SELECT * FROM tempRegimes
                WHERE NOT EXISTS (SELECT * FROM Regimes WHERE tempRegimes.Id = Regimes.Id);
                UPDATE Regimes SET MinSinteringTime = (SELECT MinSinteringTime FROM tempRegimes WHERE tempRegimes.Id = Regimes.Id);
                UPDATE Regimes SET MaxSinteringTime = (SELECT MaxSinteringTime FROM tempRegimes WHERE tempRegimes.Id = Regimes.Id);
                UPDATE Regimes SET MinFinalTempretare = (SELECT MinFinalTempretare FROM tempRegimes WHERE tempRegimes.Id = Regimes.Id);
                UPDATE Regimes SET MaxFinalTempretare = (SELECT MaxFinalTempretare FROM tempRegimes WHERE tempRegimes.Id = Regimes.Id);
                UPDATE Regimes SET MinCuringTime = (SELECT MinCuringTime FROM tempRegimes WHERE tempRegimes.Id = Regimes.Id);
                UPDATE Regimes SET MaxCuringTime = (SELECT MaxCuringTime FROM tempRegimes WHERE tempRegimes.Id = Regimes.Id);
                UPDATE Regimes SET MinGasPressure = (SELECT MinGasPressure FROM tempRegimes WHERE tempRegimes.Id = Regimes.Id);
                UPDATE Regimes SET MaxGasPressure = (SELECT MaxGasPressure FROM tempRegimes WHERE tempRegimes.Id = Regimes.Id);");
        }
    }
}