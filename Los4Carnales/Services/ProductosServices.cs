using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class ProductosServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int idProducto)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Producto.AnyAsync(c => c.ProductoId == idProducto);
    }

    public async Task<Producto?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Producto.FirstOrDefaultAsync(e => e.ProductoId == id);
    }

    public async Task<List<Producto>> Listar(Expression<Func<Producto, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Producto.Where(criterio).AsNoTracking().ToListAsync();
    }

    public async Task<bool> Insertar(Producto producto)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Producto.Add(producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Producto producto)
    {
        if (!await Existe(producto.ProductoId))
            return await Insertar(producto);
        else
            return await Modificar(producto);
    }

    public async Task<bool> Modificar(Producto producto)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    // --- NUEVOS MÉTODOS DE ELIMINACIÓN LÓGICA ---

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var producto = await contexto.Producto.FindAsync(id);

        if (producto == null) return false;

        producto.Eliminado = true; // Marcado lógico
        contexto.Update(producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Producto>> ListarPapelera()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Producto
            .IgnoreQueryFilters() // Para traer los registros con Eliminado = true
            .Where(p => p.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Restaurar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var producto = await contexto.Producto
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.ProductoId == id);

        if (producto == null) return false;

        producto.Eliminado = false;
        contexto.Update(producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> EliminarPermanente(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Producto
            .IgnoreQueryFilters()
            .Where(p => p.ProductoId == id)
            .ExecuteDeleteAsync() > 0;
    }

    // --- CONSULTAS ESPECIALIZADAS ---

    public async Task<List<Producto>> ListarProductosBajoStock(int limite = 10)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Producto
            // Filtramos para que no salgan productos que están en la papelera
            .Where(p => p.Existencia <= limite && !p.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }
}