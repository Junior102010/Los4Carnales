using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class CategoriasService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int IdCategoria)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Categoria.AnyAsync(c => c.CategoriaId == IdCategoria);
    }

    public async Task<Categorias?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Categoria.FirstOrDefaultAsync(e => e.CategoriaId == id);
    }

    public async Task<List<Categorias>> Listar(Expression<Func<Categorias, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Categoria.Where(criterio).AsNoTracking().ToListAsync();
    }

    public async Task<bool> Insertar(Categorias categoria)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Categoria.Add(categoria);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Categorias categoria)
    {
        if (!await Existe(categoria.CategoriaId))
            return await Insertar(categoria);
        else
            return await Modificar(categoria);
    }

    public async Task<bool> Modificar(Categorias categoria)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(categoria);
        return await contexto.SaveChangesAsync() > 0;
    }

    // --- NUEVOS MÉTODOS DE ELIMINACIÓN LÓGICA ---

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var categoria = await contexto.Categoria.FindAsync(id);

        if (categoria == null) return false;

        categoria.Eliminado = true; // Marcado lógico
        contexto.Update(categoria);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Categorias>> ListarPapelera()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Categoria
            .IgnoreQueryFilters() // Para traer los registros con Eliminado = true
            .Where(p => p.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Restaurar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var categoria = await contexto.Categoria
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.CategoriaId == id);

        if (categoria == null) return false;

        categoria.Eliminado = false;
        contexto.Update(categoria);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> EliminarPermanente(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Categoria
            .IgnoreQueryFilters()
            .Where(p => p.CategoriaId == id)
            .ExecuteDeleteAsync() > 0;
    }

    // --- CONSULTAS ESPECIALIZADAS ---

    public async Task<List<Categorias>> ListarProductosBajoStock(int limite = 10)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.
            // Filtramos para que no salgan productos que están en la papelera
            .Where(p => p.Existencia <= limite && !p.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }
}