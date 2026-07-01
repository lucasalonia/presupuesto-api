using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace presupuesto_api.Migrations
{
    /// <inheritdoc />
    public partial class ModPropiedadPresupuesto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proyecciones_Proyecciones_ProyeccionId",
                table: "Proyecciones");

            migrationBuilder.DropIndex(
                name: "IX_Proyecciones_ProyeccionId",
                table: "Proyecciones");

            migrationBuilder.DropColumn(
                name: "ProyeccionId",
                table: "Proyecciones");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "Proyecciones",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Proyecciones",
                keyColumn: "descripcion",
                keyValue: null,
                column: "descripcion",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "Proyecciones",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ProyeccionId",
                table: "Proyecciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proyecciones_ProyeccionId",
                table: "Proyecciones",
                column: "ProyeccionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proyecciones_Proyecciones_ProyeccionId",
                table: "Proyecciones",
                column: "ProyeccionId",
                principalTable: "Proyecciones",
                principalColumn: "id_proyeccion");
        }
    }
}
