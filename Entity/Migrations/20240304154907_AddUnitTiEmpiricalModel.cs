using Entity.Migrations.PostDeploymentScripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitTiEmpiricalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitAlias",
                table: "EmpiricalModelTypes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            AddEmpiricalModels.AddUpScripts(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitAlias",
                table: "EmpiricalModelTypes");
        }
    }
}
