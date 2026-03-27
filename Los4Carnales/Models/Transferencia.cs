
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

    [Required(ErrorMessage = "El monto es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public double Monto { get; set; } // Cambiado a decimal para precisión financiera

    public string? Observaciones { get; set; } // Opcional para mayor flexibilidad

    public bool Eliminado { get; set; } = false;

    // Relación con Cliente (Necesaria para el registro del Admin)
    public int ClienteId { get; set; }

    public int? PedidoId { get; set; }

    [ForeignKey("PedidoId")]
    public Pedido? Pedido { get; set; }

    public ICollection<TransferenciaImagen> Imagenes { get; set; } = new List<TransferenciaImagen>();

}
