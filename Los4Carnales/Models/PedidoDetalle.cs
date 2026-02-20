using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Los4Carnales.Models;

public class PedidoDetalle
{
    [Key]
    public int DetalleId { get; set; }

    public int PedidoId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un producto")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto de la lista")]
    public int ProductoId { get; set; }

    [ForeignKey("ProductoId")]
    public Producto? Producto { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria")]
    [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public double Cantidad { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public double PrecioUnitario { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "El importe no puede ser negativo")]
    public double Importe { get; set; }
    public bool Eliminado { get; set; } = false;
}