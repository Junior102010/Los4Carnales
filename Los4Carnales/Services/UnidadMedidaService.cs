using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services
{
    public class UnidadMedidaService(IDbContextFactory<ApplicationDbContext> DbFactory)
    {
        public async Task<bool> Existe(int idUnidad)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.UnidadMedida
                .AnyAsync(u => u.UnidadId == idUnidad);
        }

        public async Task<bool> ExisteDescripcion(string descripcion, int idExcluir = 0)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.UnidadMedida
                .AnyAsync(u => u.Descripcion.ToLower() == descripcion.ToLower()
                            && u.UnidadId != idExcluir);
        }

        public async Task<UnidadMedida?> Buscar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.UnidadMedida
                .FirstOrDefaultAsync(u => u.UnidadId == id);
        }

        public async Task<List<UnidadMedida>> Listar(Expression<Func<UnidadMedida, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.UnidadMedida
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> Insertar(UnidadMedida unidad)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.UnidadMedida.Add(unidad);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Guardar(UnidadMedida unidad)
        {
            if (!await Existe(unidad.UnidadId))
                return await Insertar(unidad);
            else
                return await Modificar(unidad);
        }

        public async Task<bool> Modificar(UnidadMedida unidad)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(unidad);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            var unidad = await contexto.UnidadMedida.FindAsync(id);
            if (unidad == null)
                return false;

            unidad.Eliminado = true;
            contexto.Update(unidad);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<List<UnidadMedida>> ListarPapelera()
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.UnidadMedida
                .IgnoreQueryFilters()
                .Where(u => u.Eliminado)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> Restaurar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            var unidad = await contexto.UnidadMedida
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.UnidadId == id);

            if (unidad == null)
                return false;

            unidad.Eliminado = false;
            contexto.Update(unidad);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarPermanente(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            return await contexto.UnidadMedida
                .IgnoreQueryFilters()
                .Where(u => u.UnidadId == id)
                .ExecuteDeleteAsync() > 0;
        }
    }
}