using Entity.Migrations.PostDeploymentScripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class FixTablesRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmpiricalModelCoeffs_EmpiricalModels_EmpiricalModelId",
                table: "EmpiricalModelCoeffs");

            migrationBuilder.DropForeignKey(
                name: "FK_ParamsRanges_EmpiricalModels_ModelId",
                table: "ParamsRanges");

            migrationBuilder.AlterColumn<int>(
                name: "ModelId",
                table: "ParamsRanges",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<double>(
                name: "Step",
                table: "ParamsRanges",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "EmpiricalModelId",
                table: "EmpiricalModelCoeffs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_EmpiricalModelCoeffs_EmpiricalModels_EmpiricalModelId",
                table: "EmpiricalModelCoeffs",
                column: "EmpiricalModelId",
                principalTable: "EmpiricalModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParamsRanges_EmpiricalModels_ModelId",
                table: "ParamsRanges",
                column: "ModelId",
                principalTable: "EmpiricalModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmpiricalModelCoeffs_EmpiricalModels_EmpiricalModelId",
                table: "EmpiricalModelCoeffs");

            migrationBuilder.DropForeignKey(
                name: "FK_ParamsRanges_EmpiricalModels_ModelId",
                table: "ParamsRanges");

            migrationBuilder.DropColumn(
                name: "Step",
                table: "ParamsRanges");

            migrationBuilder.AlterColumn<int>(
                name: "ModelId",
                table: "ParamsRanges",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmpiricalModelId",
                table: "EmpiricalModelCoeffs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmpiricalModelCoeffs_EmpiricalModels_EmpiricalModelId",
                table: "EmpiricalModelCoeffs",
                column: "EmpiricalModelId",
                principalTable: "EmpiricalModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParamsRanges_EmpiricalModels_ModelId",
                table: "ParamsRanges",
                column: "ModelId",
                principalTable: "EmpiricalModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
