using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Los4Carnales.Models;

namespace Los4Carnales.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Pedido> Pedido { get; set; }

    public DbSet<Usuario> Usuario { get; set; }

    public DbSet<Producto> Producto { get; set; }

    public DbSet<Entrada> Entrada { get; set; }

    public DbSet<Cliente> Cliente { get; set; }


    public DbSet<TransferenciaImagen> TransferenciaImagenes { get; set; }

    public DbSet<Categorias> Categoria { get; set; }

    public DbSet<Transferencia> Transferencia { get; set; }

    public DbSet<EntradaDetalle> EntradaDetalles { get; set; }
    public DbSet<Abono> Abono { get; set; }
    public DbSet<UnidadMedida> UnidadMedida { get; set; }
    public DbSet<Sector> Sector { get; set; }
    public DbSet<Factura> Factura { get; set; }
    public DbSet<Proveedores> Proveedores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>()
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Cliente>(c => c.UserId)
            .IsRequired();

        modelBuilder.Entity<Proveedores>().HasQueryFilter(p => !p.Eliminado);
        modelBuilder.Entity<Pedido>().HasQueryFilter(p => !p.Eliminado);
        modelBuilder.Entity<Producto>().HasQueryFilter(p => !p.Eliminado);
        modelBuilder.Entity<Entrada>().HasQueryFilter(e => !e.Eliminado);
        modelBuilder.Entity<Cliente>().HasQueryFilter(c => !c.Eliminado);
        modelBuilder.Entity<Categorias>().HasQueryFilter(c => !c.Eliminado);
        modelBuilder.Entity<Transferencia>().HasQueryFilter(t => !t.Eliminado);
        modelBuilder.Entity<Abono>().HasQueryFilter(a => !a.Eliminado);
        modelBuilder.Entity<UnidadMedida>().HasQueryFilter(u => !u.Eliminado);
        modelBuilder.Entity<Sector>().HasQueryFilter(s => !s.Eliminado);
        modelBuilder.Entity<Factura>().HasQueryFilter(f => !f.Eliminado);

        modelBuilder.Entity<EntradaDetalle>()
        .HasOne(ed => ed.Producto)
        .WithMany()
        .HasForeignKey(ed => ed.ProductoId)
        .IsRequired(false); // Esto evita que truene si el producto está "oculto" por el filtro

        modelBuilder.Entity<PedidoDetalle>()
            .HasOne(pd => pd.Producto)
            .WithMany()
            .HasForeignKey(pd => pd.ProductoId)
            .IsRequired(false);
    }
    

}
