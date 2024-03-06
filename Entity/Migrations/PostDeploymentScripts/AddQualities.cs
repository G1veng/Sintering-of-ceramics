using Microsoft.EntityFrameworkCore.Migrations;

namespace Entity.Migrations.PostDeploymentScripts
{
    internal class AddQualities
    {
        public static void AddUpScripts(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TABLE IF EXISTS tempQualities;
                CREATE TEMP TABLE tempQualities AS SELECT * FROM Qualities;
                INSERT INTO tempQualities (Id, Unit, Alias, UnitAlias) VALUES
                (1, 0, 'Конечная пористость', '%'),
                (2, 0, 'Конечная плотность', 'кг/см³'),
                (3, 0, 'Средний размер зерна', 'мкм'),
                (4, 0, 'Плотность твердого сплава', 'кг/см³'),
                (5, 0, 'Прочность твердого сплава при поперечном изгибе', 'МПа'),
                (6, 0, 'Остаточная пористость твердого сплава', '%'),
                (7, 0, 'Твердость сплава', 'кгс/мм²');
                INSERT INTO Qualities SELECT * FROM tempQualities
                WHERE NOT EXISTS (SELECT * FROM Qualities WHERE tempQualities.Id = Qualities.Id);
                UPDATE Qualities SET Alias = (SELECT Alias FROM tempQualities WHERE tempQualities.Id = Qualities.Id);
                UPDATE Qualities SET UnitAlias = (SELECT UnitAlias FROM tempQualities WHERE tempQualities.Id = Qualities.Id);");
        }
    }
}
