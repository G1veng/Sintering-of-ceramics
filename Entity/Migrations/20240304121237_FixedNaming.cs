using Entity.Migrations.PostDeploymentScripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class FixedNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Furmula",
                table: "EmpiricalModels",
                newName: "Formula");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Formula",
                table: "EmpiricalModels",
                newName: "Furmula");
        }
    }
}
