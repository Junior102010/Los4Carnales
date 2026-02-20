
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Los4Carnales.Models;

public class Transferencia
{
    [Key]
    public int TransferenciaId { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Campo obligatorio")]
    public string Origen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obligatorio")]
    public string Destino { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tiene que Tener un Monto 😪")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto transferido debe ser mayor a 0.")]
    public double Monto { get; set; }

    [Required]
    public string Observaciones { get; set; } = string.Empty;
    public bool Eliminado { get; set; } = false;
    public int? ClienteId { get; set; }

    [ForeignKey("ClienteId")]
    public Cliente? Cliente { get; set; }

    public int? PedidoId { get; set; }

    [ForeignKey("PedidoId")]
    public Pedido? Pedido { get; set; }

    public ICollection<TransferenciaImagen> Imagenes { get; set; } = new List<TransferenciaImagen>();
  
}
