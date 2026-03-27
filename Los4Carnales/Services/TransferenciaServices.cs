using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class TranferenciaServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int idTransferencia)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Transferencia.AnyAsync(c => c.TransferenciaId == idTransferencia);
    }

    public async Task<Transferencia?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        return await contexto.Transferencia
            .Include(t => t.Imagenes)
            .FirstOrDefaultAsync(c => c.TransferenciaId == id);
    }

    public async Task<List<Transferencia>> Listar(Expression<Func<Transferencia, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        return await contexto.Transferencia
            .Include(t => t.Imagenes)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Insertar(Transferencia transferencia)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var nuevaTransferencia = new Transferencia
        {
            Fecha = transferencia.Fecha,
            Origen = transferencia.Origen,
            Destino = transferencia.Destino,
            Monto = transferencia.Monto,
            Observaciones = transferencia.Observaciones,
            Imagenes = new List<TransferenciaImagen>()
        };

        contexto.Transferencia.Add(nuevaTransferencia);
        var guardado = await contexto.SaveChangesAsync() > 0;

        if (guardado && transferencia.Imagenes != null && transferencia.Imagenes.Any())
        {
            foreach (var img in transferencia.Imagenes)
            {
                img.TransferenciaId = nuevaTransferencia.TransferenciaId;
                contexto.TransferenciaImagenes.Add(img);
            }
            await contexto.SaveChangesAsync();
        }

        return guardado;
    }

    public async Task<bool> Guardar(Transferencia transferencia)
    {
        if (!await Existe(transferencia.TransferenciaId))
        {
            return await Insertar(transferencia);
        }
        else
        {
            return await Modificar(transferencia);
        }
    }

    public async Task<bool> Modificar(Transferencia transferencia)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var imagenesActuales = contexto.TransferenciaImagenes
            .Where(ti => ti.TransferenciaId == transferencia.TransferenciaId);

        contexto.TransferenciaImagenes.RemoveRange(imagenesActuales);

        contexto.Update(transferencia);

        return await contexto.SaveChangesAsync() > 0;
    }

    // --- NUEVA LÓGICA DE ELIMINACIÓN LÓGICA ---

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var transferencia = await contexto.Transferencia.FindAsync(id);

        if (transferencia == null)
            return false;

        transferencia.Eliminado = true; // Basado en la lógica de ProveedoresServices
        contexto.Update(transferencia);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Transferencia>> ListarPapelera()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Transferencia
            .IgnoreQueryFilters() // Para ver los registros con Eliminado = true
            .Where(t => t.Eliminado)
            .Include(t => t.Imagenes)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Restaurar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var transferencia = await contexto.Transferencia
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.TransferenciaId == id);

        if (transferencia == null)
            return false;

        transferencia.Eliminado = false; // Restaurar registro
        contexto.Update(transferencia);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> EliminarPermanente(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var transferencia = await contexto.Transferencia
            .IgnoreQueryFilters()
            .Include(t => t.Imagenes)
            .FirstOrDefaultAsync(t => t.TransferenciaId == id);

        if (transferencia == null)
            return false;

        // Eliminar imágenes relacionadas primero si es necesario
        if (transferencia.Imagenes != null)
            contexto.TransferenciaImagenes.RemoveRange(transferencia.Imagenes);

        contexto.Transferencia.Remove(transferencia); // Borrado físico de la BD
        return await contexto.SaveChangesAsync() > 0;
    }

}