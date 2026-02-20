using Microsoft.AspNetCore.Identity;

namespace Los4Carnales.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public string Telefono { get; set; }
}
