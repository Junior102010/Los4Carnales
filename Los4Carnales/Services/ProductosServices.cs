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
        {

            return await Insertar(producto);
        }
        else
        {
            return await Modificar(producto);
        }
    }

    public async Task<bool> Modificar(Producto producto)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        
        contexto.Update(producto);

        
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Producto>> ListarProductosBajoStock(int limite = 10)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Producto
            .Where(p => p.Existencia <= limite)
            .AsNoTracking()
            .ToListAsync();
    }

}
