using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class ProveedoresServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int idProveedor)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Proveedores.AnyAsync(c => c.ProveedorId == idProveedor);
    }
    public async Task<bool> ExisteNombre(string nombre, int idExcluir = 0)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Proveedores
            .AnyAsync(p => p.Nombre.ToLower() == nombre.ToLower() && p.ProveedorId != idExcluir);
    }

    public async Task<Proveedores?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        return await contexto.Proveedores.FirstOrDefaultAsync(e => e.ProveedorId == id);
    }

    public async Task<List<Proveedores>> Listar(Expression<Func<Proveedores, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Proveedores.Where(criterio).AsNoTracking().ToListAsync();

    }

    public async Task<bool> Insertar(Proveedores proveedor)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Proveedores.Add(proveedor);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Proveedores proveedor)
    {
        if (!await Existe(proveedor.ProveedorId))
        {

            return await Insertar(proveedor);
        }
        else
        {
            return await Modificar(proveedor);
        }
    }

    public async Task<bool> Modificar(Proveedores proveedor)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(proveedor);
        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var proveedor = await contexto.Proveedores.FindAsync(id);
        if (proveedor == null)
            return false;
        proveedor.Eliminado = true;
        contexto.Update(proveedor);
        return await contexto.SaveChangesAsync() > 0;
    }

    //Eliminado logico
    public async Task<List<Proveedores>> ListarPapelera()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Proveedores
            .IgnoreQueryFilters()
            .Where(p => p.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Restaurar(int id) 
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var proveedor = await contexto.Proveedores
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.ProveedorId == id);

        if (proveedor == null)
            return false;
        proveedor.Eliminado = false;
        contexto.Proveedores.Update(proveedor);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> EliminarPermanente(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        return await contexto.Proveedores
            .IgnoreQueryFilters()
            .Where(p => p.ProveedorId == id)
            .ExecuteDeleteAsync() > 0;

    }

}
