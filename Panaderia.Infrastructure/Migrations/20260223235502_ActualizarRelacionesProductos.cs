using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Panaderia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarRelacionesProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recetas_MateriasPrimas_MateriaPrimaId",
                table: "Recetas");

            migrationBuilder.AddForeignKey(
                name: "FK_Recetas_MateriasPrimas_MateriaPrimaId",
                table: "Recetas",
                column: "MateriaPrimaId",
                principalTable: "MateriasPrimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recetas_MateriasPrimas_MateriaPrimaId",
                table: "Recetas");

            migrationBuilder.AddForeignKey(
                name: "FK_Recetas_MateriasPrimas_MateriaPrimaId",
                table: "Recetas",
                column: "MateriaPrimaId",
                principalTable: "MateriasPrimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
