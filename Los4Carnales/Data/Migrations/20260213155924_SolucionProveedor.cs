using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Los4Carnales.Migrations
{
    /// <inheritdoc />
    public partial class SolucionProveedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categorias_proveedores_CategoriaId",
                table: "categorias");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "categorias",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "categorias",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_categorias_proveedores_CategoriaId",
                table: "categorias",
                column: "CategoriaId",
                principalTable: "proveedores",
                principalColumn: "ProveedorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
