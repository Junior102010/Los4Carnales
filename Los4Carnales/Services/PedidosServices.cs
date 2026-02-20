using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class PedidosServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int idPedido)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedido.AnyAsync(c => c.PedidoId == idPedido);
    }

    public async Task<Pedido?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedido
            .Include(p => p.Cliente)
            .Include(p => p.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(e => e.PedidoId == id);
    }

    public async Task<List<Pedido>> Listar(Expression<Func<Pedido, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedido
            .Include(p => p.Cliente)
            .Include(p => p.Detalles)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    // --- NUEVA LÓGICA DE ELIMINACIÓN LÓGICA ---

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var pedido = await contexto.Pedido
            .Include(p => p.Detalles)
            .FirstOrDefaultAsync(p => p.PedidoId == id);

        if (pedido == null) return false;

        // Si el pedido estaba entregado, devolvemos el stock al eliminarlo lógicamente
        if (pedido.Estado == "Entregado")
        {
            await AfectarExistencia(contexto, pedido.Detalles.ToList(), true);
        }

        pedido.Eliminado = true;
        contexto.Update(pedido);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Pedido>> ListarPapelera()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedido
            .IgnoreQueryFilters()
            .Include(p => p.Cliente)
            .Where(p => p.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Restaurar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var pedido = await contexto.Pedido
            .IgnoreQueryFilters()
            .Include(p => p.Detalles)
            .FirstOrDefaultAsync(p => p.PedidoId == id);

        if (pedido == null) return false;

        // Si el pedido estaba entregado, volvemos a descontar el stock al restaurarlo
        if (pedido.Estado == "Entregado")
        {
            await AfectarExistencia(contexto, pedido.Detalles.ToList(), false);
        }

        pedido.Eliminado = false;
        contexto.Update(pedido);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> EliminarPermanente(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        // Nota: El borrado permanente no afecta stock porque ya se afectó en el borrado lógico inicial
        return await contexto.Pedido
            .IgnoreQueryFilters()
            .Where(p => p.PedidoId == id)
            .ExecuteDeleteAsync() > 0;
    }

    // --- MÉTODOS EXISTENTES (AJUSTADOS) ---

    public async Task<bool> Guardar(Pedido pedido)
    {
        if (!await Existe(pedido.PedidoId))
            return await Insertar(pedido);
        else
            return await Modificar(pedido);
    }

    private async Task<bool> Insertar(Pedido pedido)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Pedido.Add(pedido);
        var guardado = await contexto.SaveChangesAsync() > 0;

        if (!guardado) return false;

        var nuevaFactura = new Factura
        {
            PedidoId = pedido.PedidoId,
            ClienteId = pedido.ClienteId,
            FechaEmision = DateTime.Now,
            MontoTotal = pedido.MontoTotal,
            CodigoFactura = $"FAC-{pedido.PedidoId:D6}"
        };

        contexto.Factura.Add(nuevaFactura);
        await contexto.SaveChangesAsync();

        if (pedido.Estado == "Entregado")
        {
            await AfectarExistencia(contexto, pedido.Detalles.ToList(), false);
        }

        return true;
    }

    private async Task<bool> Modificar(Pedido pedido)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var pedidoOriginal = await contexto.Pedido
            .Include(p => p.Detalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PedidoId == pedido.PedidoId);

        if (pedidoOriginal != null && pedidoOriginal.Estado == "Entregado")
            await AfectarExistencia(contexto, pedidoOriginal.Detalles.ToList(), true);

        if (pedido.Estado == "Entregado")
            await AfectarExistencia(contexto, pedido.Detalles.ToList(), false);

        foreach (var detalle in pedido.Detalles) detalle.Producto = null;

        contexto.Update(pedido);
        return await contexto.SaveChangesAsync() > 0;
    }

    private static async Task AfectarExistencia(ApplicationDbContext contexto, List<PedidoDetalle> detalles, bool sumar)
    {
        foreach (var item in detalles)
        {
            var producto = await contexto.Producto.FindAsync(item.ProductoId);
            if (producto != null)
            {
                if (sumar) producto.Existencia += (int)item.Cantidad;
                else producto.Existencia -= (int)item.Cantidad;
            }
        }
    }

    public async Task<double> CalcularIngresosTotales()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedido
            .Where(p => p.Estado == "Entregado" && !p.Eliminado) // No contar pedidos en papelera
            .SumAsync(p => p.MontoTotal);
    }

    public async Task<int> ContarPedidosPendientes()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedido
            .CountAsync(p => (p.Estado == "Pendiente" || p.Estado == "En Proceso") && !p.Eliminado);
    }

    public async Task<Factura?> BuscarFacturaPorPedido(int pedidoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Factura.FirstOrDefaultAsync(f => f.PedidoId == pedidoId);
    }
}