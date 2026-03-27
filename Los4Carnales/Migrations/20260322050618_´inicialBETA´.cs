using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Los4Carnales.Migrations
{
    /// <inheritdoc />
    public partial class inicialBETA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MostrarVencimiento",
                table: "EntradaDetalles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeDescontoExistencia",
                table: "EntradaDetalles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MostrarVencimiento",
                table: "EntradaDetalles");

            migrationBuilder.DropColumn(
                name: "SeDescontoExistencia",
                table: "EntradaDetalles");
        }
    }
}
