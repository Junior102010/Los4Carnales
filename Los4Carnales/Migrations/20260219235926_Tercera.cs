using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Los4Carnales.Migrations
{
    /// <inheritdoc />
    public partial class Tercera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "UnidadMedida",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Transferencia",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Sector",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Producto",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "PedidoDetalle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Pedido",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Factura",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "EntradaDetalles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Entrada",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Cliente",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Categoria",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Abono",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "UnidadMedida");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Transferencia");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Sector");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "PedidoDetalle");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "EntradaDetalles");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Entrada");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Abono");
        }
    }
}
