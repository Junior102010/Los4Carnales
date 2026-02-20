using Los4Carnales.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Los4Carnales.Models;

public class Cliente
{
    [Key]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "Campo obligatorio.")]
    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;

    [Required(ErrorMessage = "Campo obligatorio.")]
    public string NombreCliente { get; set; }

    [Required(ErrorMessage = "Campo obligatorio.")]
    public string ApellidoCliente { get; set; }

    [Required(ErrorMessage = "Campo obligatorio.")]
    [RegularExpression(@"^(809|829|849)-\d{3}-\d{4}$",
        ErrorMessage = "Formato incorrecto (XXX-XXX-XXXX).")]
    public string TelefonoCliente { get; set; }

    [Required(ErrorMessage = "Campo obligatorio.")]
    public string Sector { get; set; }

    [Required(ErrorMessage = "Campo obligatorio.")]
    public string CalleCliente { get; set; }

    [Required(ErrorMessage = "Campo obligatorio.")]
    public string ViviendaCliente { get; set; }

    [Required(ErrorMessage = "Campo obligatorio.")]
    public string DescripcionCliente { get; set; }
    public bool Eliminado { get; set; } = false;

    public ICollection<Transferencia> transferencia { get; set; } = new List<Transferencia>();
}