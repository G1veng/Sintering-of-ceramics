using Microsoft.EntityFrameworkCore.Migrations;

namespace Entity.Migrations.PostDeploymentScripts
{
    internal class AddUsers
    {
        public static void AddUpScripts(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP TABLE IF EXISTS tempRoles;
                CREATE TEMP TABLE tempRoles AS SELECT * FROM Roles;
                INSERT INTO tempRoles (Id, Alias) VALUES
                (1, 'Admin'),
                (2, 'Instructor'),
                (3, 'Math model specialist'),
                (4, 'Researcher');
                INSERT INTO Roles SELECT * FROM tempRoles
                WHERE NOT EXISTS (SELECT * FROM Roles WHERE tempRoles.Id = Roles.Id);
                UPDATE Roles SET Alias = (SELECT Alias FROM tempRoles WHERE tempRoles.Id = Roles.Id);
                DROP TABLE IF EXISTS tempUsers;
                CREATE TEMP TABLE tempUsers AS SELECT * FROM Users;
                INSERT INTO tempUsers (Id, Login, Password, IsAdmin, RoleId) VALUES 
                (1, 'Admin', 'root', 1, 1),
                (2, 'User', '1', 1, 4);
                INSERT INTO Users SELECT * FROM tempUsers
                WHERE NOT EXISTS (SELECT * FROM Users WHERE tempUsers.Id = Users.Id);
                UPDATE Users SET Login = (SELECT Login FROM tempUsers WHERE tempUsers.Id = Users.Id);");
        }
    }
}
