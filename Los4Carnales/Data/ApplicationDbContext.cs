using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Los4Carnales.Models;

namespace Los4Carnales.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Proveedores> proveedores { get; set; }
        public DbSet<Categorias> categorias { get; set; }
    }
}
