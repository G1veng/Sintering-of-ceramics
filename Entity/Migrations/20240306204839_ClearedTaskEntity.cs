using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class ClearedTaskEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Regimes_RegimeId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_RegimeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "RegimeId",
                table: "Tasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegimeId",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_RegimeId",
                table: "Tasks",
                column: "RegimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Regimes_RegimeId",
                table: "Tasks",
                column: "RegimeId",
                principalTable: "Regimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
