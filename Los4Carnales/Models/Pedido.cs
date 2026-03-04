using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Los4Carnales.Models;

public class Pedido
{
    [Key]
    public int PedidoId { get; set; }

    [Required (ErrorMessage="Campo Requerido.")]
    public DateTime FechaPedido { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Campo Requerido.")]
    public int ClienteId { get; set; }
    [ForeignKey("ClienteId")]
    public Cliente? Cliente { get; set; }
    [Required(ErrorMessage = "Campo Requerido.")]
    public double MontoTotal { get; set; }
    [Required(ErrorMessage = "Campo Requerido.")]
    public string MetodoPago { get; set; } = "Efectivo";
    [Required(ErrorMessage = "Campo Requerido.")]
    public string Estado { get; set; } = "Pendiente";
    [Required(ErrorMessage = "Campo Requerido.")]
    public bool Delivery { get; set; }
    public bool Eliminado { get; set; } = false;
    [Required(ErrorMessage = "Campo Requerido.")]
    public string? ReferenciaSitio { get; set; }

    public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();

}
