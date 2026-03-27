using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services
{
    public class SectorService(IDbContextFactory<ApplicationDbContext> DbFactory)
    {
        public async Task<bool> Existe(int idSector)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Sector
                .AnyAsync(s => s.SectorId == idSector);
        }

        public async Task<bool> ExisteNombre(string nombre, int idExcluir = 0)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Sector
                .AnyAsync(s => s.Nombre.ToLower() == nombre.ToLower()
                            && s.SectorId != idExcluir);
        }

        public async Task<Sector?> Buscar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Sector
                .FirstOrDefaultAsync(s => s.SectorId == id);
        }

        public async Task<List<Sector>> Listar(Expression<Func<Sector, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Sector
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> Insertar(Sector sector)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Sector.Add(sector);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Guardar(Sector sector)
        {
            if (!await Existe(sector.SectorId))
                return await Insertar(sector);
            else
                return await Modificar(sector);
        }

        public async Task<bool> Modificar(Sector sector)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(sector);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            var sector = await contexto.Sector.FindAsync(id);
            if (sector == null)
                return false;

            sector.Eliminado = true;
            contexto.Update(sector);
            return await contexto.SaveChangesAsync() > 0;
        }

        //  Papelera
        public async Task<List<Sector>> ListarPapelera()
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Sector
                .IgnoreQueryFilters()
                .Where(s => s.Eliminado)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> Restaurar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            var sector = await contexto.Sector
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(s => s.SectorId == id);

            if (sector == null)
                return false;

            sector.Eliminado = false;
            contexto.Update(sector);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarPermanente(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            return await contexto.Sector
                .IgnoreQueryFilters()
                .Where(s => s.SectorId == id)
                .ExecuteDeleteAsync() > 0;
        }
    }
}