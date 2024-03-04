using Microsoft.EntityFrameworkCore.Migrations;

namespace Entity.Migrations.PostDeploymentScripts
{
    internal class AddEmpiricalModels
    {
        public static void AddUpScripts(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP TABLE IF EXISTS tempUnits;
            CREATE TEMP TABLE tempUnits AS SELECT * FROM ParamsRangesUnits;
            INSERT INTO tempUnits (Id, Alias) VALUES
            (1, '--'),
            (2, 'атм'),
            (3, '°C'),
            (4, 'мин');
            INSERT INTO ParamsRangesUnits SELECT * FROM tempUnits
            WHERE NOT EXISTS (SELECT * FROM ParamsRangesUnits WHERE ParamsRangesUnits.Id = tempUnits.Id);
            UPDATE ParamsRangesUnits SET Alias = (SELECT Alias FROM tempUnits WHERE tempUnits.Id = ParamsRangesUnits.Id);

            DROP TABLE IF EXISTS tempEmpiricalModelTypes;
            CREATE TEMP TABLE tempEmpiricalModelTypes AS SELECT * FROM EmpiricalModelTypes;
            INSERT INTO tempEmpiricalModelTypes (Id, Alias, UnitAlias) VALUES
            (1, 'Плотность твердого сплава', 'кг/см³'),
            (2, 'Прочность твердого сплава при поперечном изгибе', 'МПа'),
            (3, 'Остаточная пористость твердого сплава', '%'),
            (4, 'Твердость сплава', 'кгс/мм²');
            INSERT INTO EmpiricalModelTypes SELECT * FROM tempEmpiricalModelTypes
            WHERE NOT EXISTS (SELECT * FROM EmpiricalModelTypes WHERE tempEmpiricalModelTypes.Id = EmpiricalModelTypes.Id);
            UPDATE EmpiricalModelTypes SET Alias = (SELECT Alias FROM tempEmpiricalModelTypes WHERE tempEmpiricalModelTypes.Id = EmpiricalModelTypes.Id);

            DROP TABLE IF EXISTS tempEmpiricalModels;
            CREATE TEMP TABLE tempEmpiricalModels AS SELECT * FROM EmpiricalModels;
            INSERT INTO tempEmpiricalModels (Id, TypeId, MaterialId, Formula, EquipmentId) VALUES
            (1, 1, 1, 'a0+a1*Pg+a2*T+a3*Pg*T+a4*T*T+a5*Pg*T*T', 1),
            (2, 2, 1, 'b0+b1*Pg+b2*T+b3*Pg*T+b4*Pg*Pg+b5*T*T+b6*Pg*Pg*T+b7*Pg*T*T+b8*Pg*Pg*T*T', 1),
            (3, 3, 1, 'c0+c1*T+c2*tao+c3*T*tao+c4*T*T+c5*tao*tao+c6*T*T*tao+c7*T*tao*tao+c8*T*T*tao*tao', 1),
            (4, 4, 1, 'd0+d1*tao+d2*T+d3*T*tao+d4*tao*tao+d5*T*T+d6*tao*tao*T+d7*tao*T*T+d8*T*T*tao*tao', 1);
            INSERT INTO EmpiricalModels SELECT * FROM tempEmpiricalModels
            WHERE NOT EXISTS (SELECT * FROM EmpiricalModels WHERE tempEmpiricalModels.Id = EmpiricalModels.Id);
            UPDATE EmpiricalModels SET TypeId = (SELECT TypeId FROM tempEmpiricalModels WHERE tempEmpiricalModels.Id = EmpiricalModels.Id);
            UPDATE EmpiricalModels SET MaterialId = (SELECT MaterialId FROM tempEmpiricalModels WHERE tempEmpiricalModels.Id = EmpiricalModels.Id);
            UPDATE EmpiricalModels SET Formula = (SELECT Formula FROM tempEmpiricalModels WHERE tempEmpiricalModels.Id = EmpiricalModels.Id);
            UPDATE EmpiricalModels SET EquipmentId = (SELECT EquipmentId FROM tempEmpiricalModels WHERE tempEmpiricalModels.Id = EmpiricalModels.Id);

            DROP TABLE IF EXISTS tempParamsRanges;
            CREATE TEMP TABLE tempParamsRanges AS SELECT * FROM ParamsRanges;
            INSERT INTO tempParamsRanges (Id, ModelId, UnitId, MaxValue, MinValue, Step) VALUES
            (1, 1, 2, 8, 4, 2),
            (2, 1, 3, 1500, 1300, 10),
            (3, 2, 2, 8, 4, 2),
            (4, 2, 3, 1500, 1300, 10),
            (5, 3, 3, 1550, 1300, 10),
            (6, 3, 4, 60, 30, 2),
            (7, 4, 3, 1450, 1200, 10),
            (8, 4, 4, 70, 30, 1);
            INSERT INTO ParamsRanges SELECT * FROM tempParamsRanges
            WHERE NOT EXISTS (SELECT * FROM ParamsRanges WHERE tempParamsRanges.Id = ParamsRanges.Id);
            UPDATE ParamsRanges SET UnitId = (SELECT UnitId FROM tempParamsRanges WHERE tempParamsRanges.Id = ParamsRanges.Id);
            UPDATE ParamsRanges SET MaxValue = (SELECT MaxValue FROM tempParamsRanges WHERE tempParamsRanges.Id = ParamsRanges.Id);
            UPDATE ParamsRanges SET MinValue = (SELECT MinValue FROM tempParamsRanges WHERE tempParamsRanges.Id = ParamsRanges.Id);
            UPDATE ParamsRanges SET Step = (SELECT Step FROM tempParamsRanges WHERE tempParamsRanges.Id = ParamsRanges.Id);

            DROP TABLE IF EXISTS tempEmpiricalModelCoeffs;
            CREATE TEMP TABLE tempEmpiricalModelCoeffs AS SELECT * FROM EmpiricalModelCoeffs;
            INSERT INTO tempEmpiricalModelCoeffs (Id, Alias, Value, EmpiricalModelId) VALUES
            (1, 'c0', 199.4, 3),
            (2, 'c1', -0.2765, 3),
            (3, 'c2', -4.486, 3),
            (4, 'c3', 0.0062, 3),
            (5, 'c4', 0.00096, 3),
            (6, 'c5', 0.0449, 3),
            (7, 'c6', -0.000002, 3),
            (8, 'c7', -0.000062, 3),
            (9, 'c8', -0.00000002, 3),
            (10, 'd0', 14.64, 4),
            (11, 'd1', 0.01683, 4),
            (12, 'd2', 0.1033, 4),
            (13, 'd3', 0.00012, 4),
            (14, 'd4', -0.00014, 4),
            (15, 'd5', -0.000034, 4),
            (16, 'd6', -0.00000097, 4),
            (17, 'd7', -0.00000004, 4),
            (18, 'd8', 0.00000000003, 4),
            (19, 'a0', -17.46, 1),
            (20, 'a1', -0.00622, 1),
            (21, 'a2', 0.04293, 1),
            (22, 'a3', 0.000015, 1),
            (23, 'a4', -0.000014, 1),
            (24, 'a5', -0.000000005, 1),
            (25, 'b0', -58231, 2),
            (26, 'b1', 673.2, 2),
            (27, 'b2', 81.76, 2),
            (28, 'b3', -0.9453, 2),
            (29, 'b4', -13.10, 2),
            (30, 'b5',-0.02666, 2),
            (31, 'b6', 0.0184, 2),
            (32, 'b7', 0.00031, 2),
            (33, 'b8', -0.000006, 2);
            INSERT INTO EmpiricalModelCoeffs SELECT * FROM tempEmpiricalModelCoeffs
            WHERE NOT EXISTS (SELECT * FROM EmpiricalModelCoeffs WHERE tempEmpiricalModelCoeffs.Id = EmpiricalModelCoeffs.Id);
            UPDATE EmpiricalModelCoeffs SET Alias = (SELECT Alias FROM tempEmpiricalModelCoeffs WHERE tempEmpiricalModelCoeffs.Id = EmpiricalModelCoeffs.Id);
            UPDATE EmpiricalModelCoeffs SET Value = (SELECT Value FROM tempEmpiricalModelCoeffs WHERE tempEmpiricalModelCoeffs.Id = EmpiricalModelCoeffs.Id);");
        }
    }
}
