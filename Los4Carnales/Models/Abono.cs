using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Los4Carnales.Models;


public class Abono
{
    [Key]
    public int AbonoId { get; set; }

    [Required(ErrorMessage = "Seleccione el cliente")]
    public int ClienteId { get; set; }

    [ForeignKey("ClienteId")]
    public Cliente? Cliente { get; set; }

    [Required(ErrorMessage = "El monto es obligatorio")]
    [Range(0.01, 10000000, ErrorMessage = "Monto inválido")]
    public double Monto { get; set; }

    public string Observacion { get; set; } = string.Empty;

    public DateTime Fecha { get; set; } = DateTime.Now;

    public bool Eliminado { get; set; } = false;
}