using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Los4Carnales.Models;

public class EntradaDetalle
{
    [Key]
    public int DetalleId { get; set; }

    [Required(ErrorMessage = "El ID de la Entrada es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID de la Entrada debe ser un valor positivo.")]
    public int EntradaId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un producto.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del Producto debe ser un valor positivo.")]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "Fecha de vencimiento es obligatoria")]
    public DateTime FechaVencimiento { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La Cantidad debe ser mayor que cero.")]
    public int Cantidad { get; set; }

    public bool Eliminado { get; set; } = false;

    public bool MostrarVencimiento { get; set; } = true;

    public bool SeDescontoExistencia { get; set; } = false;

    [ForeignKey("ProductoId")]
    public Producto? Producto { get; set; }

    public Entrada? Entrada { get; set; }
}