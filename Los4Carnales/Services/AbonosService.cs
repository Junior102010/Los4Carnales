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
        contexto.Abono.Add(abono);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Abono>> Listar()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Abono.Include(a => a.Cliente).AsNoTracking().ToListAsync();
    }

    public async Task<double> CalcularDeuda(int clienteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var credito = await contexto.Pedido.Where(p => p.ClienteId == clienteId && p.MetodoPago == "Credito").SumAsync(p => p.MontoTotal);
        var pagado = await contexto.Abono.Where(a => a.ClienteId == clienteId).SumAsync(a => a.Monto);
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
                double totalAbonado = await contexto.Abono
                    .Where(a => a.ClienteId == c.ClienteId)
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