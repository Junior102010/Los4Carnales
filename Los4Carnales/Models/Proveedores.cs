using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Los4Carnales.Models;

public class Proveedores
{
    [Key]
    public int ProveedorId { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public string Dirrecion { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^(809|829|849)-\d{3}-\d{4}$",
        ErrorMessage = "El teléfono debe tener el formato 809-000-0000 y comenzar con 809, 829 o 849.")]
    public string Telefono { get; set; }

    [ForeignKey("CategoriaId")]
    public ICollection<Categorias> ProveedorDetalle { get; set; } = new List<Categorias>();
}

