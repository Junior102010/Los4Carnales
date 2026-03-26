using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services
{
    public class CategoriaService(IDbContextFactory<ApplicationDbContext> DbFactory)
    {
        public async Task<bool> Existe(int idCategoria)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Categoria
                .AnyAsync(c => c.CategoriaId == idCategoria);
        }

        public async Task<bool> ExisteNombre(string nombre, int idExcluir = 0)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Categoria
                .AnyAsync(c => c.Nombre!.ToLower() == nombre.ToLower()
                            && c.CategoriaId != idExcluir);
        }

        public async Task<Categorias?> Buscar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Categoria
                .FirstOrDefaultAsync(c => c.CategoriaId == id);
        }

        public async Task<List<Categorias>> Listar(Expression<Func<Categorias, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Categoria
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
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

        public async Task<bool> Eliminar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            var categoria = await contexto.Categoria.FindAsync(id);
            if (categoria == null)
                return false;

            categoria.Eliminado = true;
            contexto.Update(categoria);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<List<Categorias>> ListarPapelera()
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Categoria
                .IgnoreQueryFilters()
                .Where(c => c.Eliminado)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> Restaurar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            var categoria = await contexto.Categoria
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.CategoriaId == id);

            if (categoria == null)
                return false;

            categoria.Eliminado = false;
            contexto.Update(categoria);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarPermanente(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            return await contexto.Categoria
                .IgnoreQueryFilters()
                .Where(c => c.CategoriaId == id)
                .ExecuteDeleteAsync() > 0;
        }
    }
}