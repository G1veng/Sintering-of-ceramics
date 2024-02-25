using Microsoft.EntityFrameworkCore.Migrations;

namespace Entity.Migrations.PostDeploymentScripts
{
    internal class AddUsers
    {
        public static void AddUpScripts(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP TABLE IF EXISTS tempUsers;
                CREATE TEMP TABLE tempUsers AS SELECT * FROM Users;
                INSERT INTO tempUsers (Id, Login, Password, IsAdmin) VALUES 
                (1, 'Admin', 'root', 1, null),
                (2, 'User', '1', 0, null);
                INSERT INTO Users SELECT * FROM tempUsers
                WHERE NOT EXISTS (SELECT * FROM Users WHERE tempUsers.Id = Users.Id);
                UPDATE Users SET Login = (SELECT Login FROM tempUsers WHERE tempUsers.Id = Users.Id);");
        }
    }
}
