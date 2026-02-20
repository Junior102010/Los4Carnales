using System.ComponentModel.DataAnnotations;

namespace Los4Carnales.Models;

public class Producto
{
    [Key]
    public int ProductoId { get; set; }
    [Required(ErrorMessage = "La etiqueta del producto es obligatorio")]
    public string Etiqueta { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio")]
    public string NombreProducto { get; set; }

    [Required(ErrorMessage = "El costo es obligatorio")]
    public double Costo { get; set; }

    [Required(ErrorMessage = "El precio para cliente es obligatorio")]
    [Range(1, 100000, ErrorMessage = "El precio debe ser mayor a 0")]

    public double Precio { get; set; }

    [Required(ErrorMessage = "El precio para empresas es obligatorio")]
    [Range(1, 100000, ErrorMessage = "El precio debe ser mayor a 0")]
    public double PrecioEmpresa { get; set; } 

    [Required(ErrorMessage = "Campo obligatorio")]
    public string UnidadMedida { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public string Categoria { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public int Existencia { get; set; }
    public bool Eliminado { get; set; } = false;
}
