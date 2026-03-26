using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Los4Carnales.Models;

public class Producto
{
    [Key]
    public int ProductoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
    public int CategoriaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una unidad de medida")]
    public int UnidadMedidaId { get; set; }

    [Required(ErrorMessage = "La etiqueta del producto es obligatoria")]
    public string? Etiqueta { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio")]
    public string? NombreProducto { get; set; }

    [Required(ErrorMessage = "El costo es obligatorio")]
    public double Costo { get; set; }

    [Required(ErrorMessage = "El precio para cliente es obligatorio")]
    [Range(1, 100000, ErrorMessage = "El precio debe ser mayor a 0")]
    public double Precio { get; set; }

    [Required(ErrorMessage = "El precio para empresas es obligatorio")]
    [Range(1, 100000, ErrorMessage = "El precio debe ser mayor a 0")]
    public double PrecioEmpresa { get; set; }

    // OJO: Quitar el [Required] de las propiedades virtuales
    [ForeignKey("UnidadMedidaId")]
    public virtual UnidadMedida? UnidadMedida { get; set; }

    [ForeignKey("CategoriaId")]
    public virtual Categorias? Categoria { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public int Existencia { get; set; }

    public bool Eliminado { get; set; } = false;

}