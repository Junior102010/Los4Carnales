using System.ComponentModel.DataAnnotations;

namespace Los4Carnales.Models;

public class UnidadMedida
{
    //CONSTRUCTOR TEMPORAL EN LO QUE "HACEMOS" LA PANTALLA UNIDAD DE MEDIDA.
    public UnidadMedida(int UnidadId, string Descripcion, string Abreviatura) 
    {
        this.UnidadId = UnidadId;
        this.Descripcion = Descripcion;
        this.Abreviatura = Abreviatura;
        Eliminado = false;
    }

    [Key]
    public int UnidadId { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    public string Descripcion { get; set; } = string.Empty; 

    [Required(ErrorMessage = "La abreviatura es obligatoria")]
    [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
    public string Abreviatura { get; set; } = string.Empty;
    public bool Eliminado { get; set; } = false;
}
