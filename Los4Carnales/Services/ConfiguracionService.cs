using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class ConfiguracionService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    // --- UNIDADES ---
    public async Task<bool> GuardarUnidad(UnidadMedida unidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        if (unidad.UnidadId == 0) contexto.UnidadMedida.Add(unidad);
        else contexto.Update(unidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<UnidadMedida>> ListarUnidades()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.UnidadMedida.AsNoTracking().ToListAsync();
    }

    // --- SECTORES ---
    public async Task<bool> GuardarSector(Sector sector)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        if (sector.SectorId == 0) contexto.Sector.Add(sector);
        else contexto.Update(sector);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Sector>> ListarSectores()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Sector.AsNoTracking().ToListAsync();
    }
    public async Task<Sector?> BuscarSector(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Sector.FindAsync(id);
    }

    public async Task<bool> EliminarSector(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var sector = await contexto.Sector.FindAsync(id);
        if (sector != null)
        {
            contexto.Sector.Remove(sector);
            return await contexto.SaveChangesAsync() > 0;
        }
        return false;
    }
    public async Task<UnidadMedida?> BuscarUnidad(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.UnidadMedida.FindAsync(id);
    }

    public async Task<bool> EliminarUnidad(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var unidad = await contexto.UnidadMedida.FindAsync(id);
        if (unidad != null)
        {
            contexto.UnidadMedida.Remove(unidad);
            return await contexto.SaveChangesAsync() > 0;
        }
        return false;
    }
}
