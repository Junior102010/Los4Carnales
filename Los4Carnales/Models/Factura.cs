using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Los4Carnales.Models;

public class Factura
{
    [Key]
    public int FacturaId { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime FechaEmision { get; set; } = DateTime.Now;
    public bool Eliminado { get; set; } = false;

    public int PedidoId { get; set; }

    [ForeignKey("PedidoId")]
    public Pedido? Pedido { get; set; }
   
    public int ClienteId { get; set; }

    [ForeignKey("ClienteId")]
    public Cliente? Cliente { get; set; }

    [DataType(DataType.Currency)]
    public double MontoTotal { get; set; }
    public string? CodigoFactura { get; set; }
}