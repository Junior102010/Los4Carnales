using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class TranferenciaServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int idTranferencia)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Transferencia.AnyAsync(c => c.TransferenciaId == idTranferencia);
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

        return await contexto.Transferencia.Include(t => t.Imagenes) .Where(criterio).AsNoTracking().ToListAsync();
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

            ClienteId = transferencia.ClienteId,

            Cliente = null,

            Imagenes = new List<TransferenciaImagen>()
        };

        contexto.Transferencia.Add(nuevaTransferencia);
        var guardado = await contexto.SaveChangesAsync() > 0;

        // Si había imágenes en el objeto original, las guardamos ahora que tenemos ID
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

        contexto.Entry(transferencia).State = EntityState.Modified;

        contexto.Update(transferencia);


        return await contexto.SaveChangesAsync() > 0;
    }

}