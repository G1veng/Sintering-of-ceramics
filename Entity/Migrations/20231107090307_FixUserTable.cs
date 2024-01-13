using Entity.Migrations.PostDeploymentScripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class FixUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "Password");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            AddUsers.AddUpScripts(migrationBuilder);
            AddOvens.AddUpScripts(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Login",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Name");
        }
    }
}
