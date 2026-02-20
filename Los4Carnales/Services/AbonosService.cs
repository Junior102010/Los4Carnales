using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class AbonosService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar(Abono abono)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        if (abono.AbonoId == 0)
            contexto.Abono.Add(abono);
        else
            contexto.Update(abono);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Abono>> Listar()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Abono
            .Include(a => a.Cliente)
            .AsNoTracking()
            .ToListAsync();
    }

    // --- NUEVOS MÉTODOS ADAPTADOS ---

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var abono = await contexto.Abono.FindAsync(id);

        if (abono == null) return false;

        abono.Eliminado = true; // Marcado lógico
        contexto.Update(abono);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Abono>> ListarPapelera()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Abono
            .IgnoreQueryFilters() // Para ver los que tienen Eliminado = true
            .Include(a => a.Cliente)
            .Where(a => a.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Restaurar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var abono = await contexto.Abono
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.AbonoId == id);

        if (abono == null) return false;

        abono.Eliminado = false;
        contexto.Update(abono);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> EliminarPermanente(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Abono
            .IgnoreQueryFilters()
            .Where(a => a.AbonoId == id)
            .ExecuteDeleteAsync() > 0;
    }

    // --- MÉTODOS DE CÁLCULO ---

    public async Task<double> CalcularDeuda(int clienteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var credito = await contexto.Pedido
            .Where(p => p.ClienteId == clienteId && p.MetodoPago == "Credito")
            .SumAsync(p => p.MontoTotal);

        // Importante: Aquí solo sumamos abonos NO eliminados
        var pagado = await contexto.Abono
            .Where(a => a.ClienteId == clienteId && !a.Eliminado)
            .SumAsync(a => a.Monto);

        return credito - pagado;
    }

    public async Task<List<ClienteDeudaDTO>> ObtenerReporteDeudas()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var clientes = await contexto.Cliente.AsNoTracking().ToListAsync();
        var listaReporte = new List<ClienteDeudaDTO>();

        foreach (var c in clientes)
        {
            double totalCredito = await contexto.Pedido
                .Where(p => p.ClienteId == c.ClienteId && p.MetodoPago == "Credito")
                .SumAsync(p => p.MontoTotal);

            if (totalCredito > 0)
            {
                // Solo sumamos abonos que no estén en la papelera
                double totalAbonado = await contexto.Abono
                    .Where(a => a.ClienteId == c.ClienteId && !a.Eliminado)
                    .SumAsync(a => a.Monto);

                double deuda = totalCredito - totalAbonado;

                if (deuda > 0 || totalCredito > 0)
                {
                    listaReporte.Add(new ClienteDeudaDTO
                    {
                        Cliente = c,
                        TotalCredito = totalCredito,
                        TotalPagado = totalAbonado,
                        DeudaPendiente = deuda
                    });
                }
            }
        }
        return listaReporte.OrderByDescending(x => x.DeudaPendiente).ToList();
    }
}

public class ClienteDeudaDTO
{
    public Cliente Cliente { get; set; } = new();
    public double TotalCredito { get; set; }
    public double TotalPagado { get; set; }
    public double DeudaPendiente { get; set; }
}