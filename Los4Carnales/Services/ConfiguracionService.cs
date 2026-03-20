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
        // Solo lista las que no están eliminadas
        return await contexto.UnidadMedida
            .Where(u => !u.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UnidadMedida?> BuscarUnidad(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.UnidadMedida.FirstOrDefaultAsync(u => u.UnidadId == id && !u.Eliminado);
    }

    public async Task<bool> EliminarUnidad(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var unidad = await contexto.UnidadMedida.FindAsync(id);
        if (unidad == null) return false;

        unidad.Eliminado = true; // Eliminado lógico
        contexto.Update(unidad);
        return await contexto.SaveChangesAsync() > 0;
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
        return await contexto.Sector
            .Where(s => !s.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Sector?> BuscarSector(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Sector.FirstOrDefaultAsync(s => s.SectorId == id && !s.Eliminado);
    }

    public async Task<bool> EliminarSector(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var sector = await contexto.Sector.FindAsync(id);
        if (sector == null) return false;

        sector.Eliminado = true; // Eliminado lógico
        contexto.Update(sector);
        return await contexto.SaveChangesAsync() > 0;
    }

    // --- LÓGICA DE PAPELERA (PARA AMBOS) ---

    public async Task<List<UnidadMedida>> ListarUnidadesPapelera()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.UnidadMedida
            .IgnoreQueryFilters()
            .Where(u => u.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Sector>> ListarSectoresPapelera()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Sector
            .IgnoreQueryFilters()
            .Where(s => s.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> RestaurarUnidad(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var unidad = await contexto.UnidadMedida.IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.UnidadId == id);

        if (unidad == null) return false;
        unidad.Eliminado = false;
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> RestaurarSector(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var sector = await contexto.Sector.IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.SectorId == id);

        if (sector == null) return false;
        sector.Eliminado = false;
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> EliminarPermanenteUnidad(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.UnidadMedida
            .IgnoreQueryFilters()
            .Where(u => u.UnidadId == id)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<bool> EliminarPermanenteSector(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Sector
            .IgnoreQueryFilters()
            .Where(s => s.SectorId == id)
            .ExecuteDeleteAsync() > 0;
    }
}