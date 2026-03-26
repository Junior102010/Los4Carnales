using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Los4Carnales.Migrations
{
    /// <inheritdoc />
    public partial class Entrada20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcuerdoPagoDias",
                table: "Entrada",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Cotizacion",
                table: "Entrada",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EsFormal",
                table: "Entrada",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaOrdenCompra",
                table: "Entrada",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrdenCompra",
                table: "Entrada",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RazonSocial",
                table: "Entrada",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RncEmpresa",
                table: "Entrada",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RncProveedor",
                table: "Entrada",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcuerdoPagoDias",
                table: "Entrada");

            migrationBuilder.DropColumn(
                name: "Cotizacion",
                table: "Entrada");

            migrationBuilder.DropColumn(
                name: "EsFormal",
                table: "Entrada");

            migrationBuilder.DropColumn(
                name: "FechaOrdenCompra",
                table: "Entrada");

            migrationBuilder.DropColumn(
                name: "OrdenCompra",
                table: "Entrada");

            migrationBuilder.DropColumn(
                name: "RazonSocial",
                table: "Entrada");

            migrationBuilder.DropColumn(
                name: "RncEmpresa",
                table: "Entrada");

            migrationBuilder.DropColumn(
                name: "RncProveedor",
                table: "Entrada");
        }
    }
}
