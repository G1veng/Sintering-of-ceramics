using Entity.Migrations.PostDeploymentScripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class AddLetterAliasToEmpiricalModelUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LetterAlias",
                table: "ParamsRangesUnits",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            AddEmpiricalModels.AddUpScripts(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LetterAlias",
                table: "ParamsRangesUnits");
        }
    }
}
