using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace presupuesto_api.Migrations
{
    /// <inheritdoc />
    public partial class ProyeccionCategoriaSetNullOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proyecciones_Categorias_id_categoria",
                table: "Proyecciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Proyecciones_Categorias_id_categoria",
                table: "Proyecciones",
                column: "id_categoria",
                principalTable: "Categorias",
                principalColumn: "id_categoria",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proyecciones_Categorias_id_categoria",
                table: "Proyecciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Proyecciones_Categorias_id_categoria",
                table: "Proyecciones",
                column: "id_categoria",
                principalTable: "Categorias",
                principalColumn: "id_categoria");
        }
    }
}
