using System.ComponentModel.DataAnnotations;

namespace Los4Carnales.Models;

public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }

    [Required]
    public string Nombre { get; set; }

}
