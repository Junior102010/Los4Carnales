using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Los4Carnales.Models;

public class Entrada
{
    [Key]
    public int EntradaId { get; set; }

    [Required(ErrorMessage = "La fecha de entrada es obligatoria.")]
    public DateTime FechaEntrada { get; set; }

    [Required(ErrorMessage = "El concepto de la entrada es obligatorio.")]
    public string Concepto { get; set; } = string.Empty;
    public string NumeroLote { get; set; } = string.Empty;
    public bool Eliminado { get; set; } = false;

    public bool EsFormal { get; set; } = false;
    public string OrdenCompra { get; set; } = string.Empty;
    public string Cotizacion { get; set; } = string.Empty;
    public DateTime? FechaOrdenCompra { get; set; }

    public string RncEmpresa { get; set; } = string.Empty;
    public string RncProveedor { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public int AcuerdoPagoDias { get; set; } = 0;


    [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
    public int ProveedorId { get; set; }

    [ForeignKey("ProveedorId")]
    public Proveedores? Proveedor { get; set; }

    [ForeignKey("EntradaId")]

    public ICollection<EntradaDetalle> EntradaDetalles { get; set; } = new List<EntradaDetalle>();
}
