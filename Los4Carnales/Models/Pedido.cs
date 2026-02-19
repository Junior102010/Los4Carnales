using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Los4Carnales.Models;

public class Pedido
{
    [Key]
    public int PedidoId { get; set; }

    [Required]
    public DateTime FechaPedido { get; set; } = DateTime.Now;
    [Required]
    public int ClienteId { get; set; }
    [ForeignKey("ClienteId")]
    public Cliente? Cliente { get; set; }
    [Required]
    public double MontoTotal { get; set; }
    [Required]
    public string MetodoPago { get; set; } = "Efectivo";
    [Required]
    public string Estado { get; set; } = "Pendiente";
    [Required]
    public bool Delivery { get; set; }
    [Required]
    public string? ReferenciaSitio { get; set; }

    public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();

}
