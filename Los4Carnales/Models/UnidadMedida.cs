using System.ComponentModel.DataAnnotations;

namespace Los4Carnales.Models;

public class UnidadMedida
{
    [Key]
    public int UnidadId { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    public string Descripcion { get; set; } = string.Empty; 

    [Required(ErrorMessage = "La abreviatura es obligatoria")]
    [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
    public string Abreviatura { get; set; } = string.Empty;
    public bool Eliminado { get; set; } = false;
}
