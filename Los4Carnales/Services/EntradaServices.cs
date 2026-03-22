using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class EntradasServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<List<Proveedores>> ListarProveedores()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        if (!await contexto.Proveedores.AnyAsync())
        {
            contexto.Proveedores.AddRange(
                new Proveedores { Nombre = "Proveedor General (Temp)", Telefono = "809-555-0001" },
                new Proveedores { Nombre = "Almacenes del Cibao", Telefono = "809-555-0002" }
            );
            await contexto.SaveChangesAsync();
        }
        return await contexto.Proveedores.AsNoTracking().ToListAsync();
    }

    public async Task<bool> Guardar(Entrada entrada)
    {
        if (!await Existe(entrada.EntradaId))
            return await Insertar(entrada);
        else
            return await Modificar(entrada);
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entrada.AnyAsync(e => e.EntradaId == id);
    }

    private async Task<bool> Insertar(Entrada entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Entrada.Add(entrada);

        // Al crear la entrada, sumamos la existencia al almacén
        await AfectarExistencia(contexto, entrada.EntradaDetalles.ToArray(), true);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Entrada entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task AfectarExistencia(ApplicationDbContext contexto, EntradaDetalle[] detalles, bool sumar)
    {
        foreach (var item in detalles)
        {
            var producto = await contexto.Producto.FindAsync(item.ProductoId);
            if (producto != null)
            {
                if (sumar)
                    producto.Existencia += item.Cantidad;
                else if (!item.SeDescontoExistencia)
                {
                    producto.Existencia -= item.Cantidad;
                    item.SeDescontoExistencia = true;
                }

                contexto.Producto.Update(producto);
            }
        }
    }
    public async Task Actualizar(EntradaDetalle detalle)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        contexto.EntradaDetalles.Update(detalle);
        await contexto.SaveChangesAsync();
    }

    public async Task AfectarExistenciaProducto(int productoId, bool sumar, int cantidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var producto = await contexto.Producto.FindAsync(productoId);
        if (producto != null)
        {
            if (sumar)
                producto.Existencia += cantidad;
            else
                producto.Existencia -= cantidad;

            contexto.Producto.Update(producto);
        }
        await contexto.SaveChangesAsync();
    }

    public async Task<Entrada?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entrada
            .Include(e => e.Proveedor)
            .Include(e => e.EntradaDetalles)
            .ThenInclude(d => d.Producto)
            .ThenInclude(d => d.UnidadMedida)
            .Include(e => e.EntradaDetalles)
            .ThenInclude(d => d.Producto)
            .ThenInclude(d => d.Categoria)
            .FirstOrDefaultAsync(e => e.EntradaId == id);
    }

    public async Task<EntradaDetalle?> BuscarEntradaDetalle(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradaDetalles
            .Include(e => e.Producto)
            .FirstOrDefaultAsync(e => e.DetalleId == id);
    }

    public async Task<List<Entrada>> Listar(Expression<Func<Entrada, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entrada
            .Include(e => e.Proveedor)
            .Include(e => e.EntradaDetalles)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<EntradaDetalle>> ListarEntradasVencidas()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        List<EntradaDetalle> entradasVencidas = new List<EntradaDetalle>();
        List<Entrada>? entradas = await contexto.Entrada
       .Include(e => e.Proveedor)
       .Include(e => e.EntradaDetalles)
       .ThenInclude(d => d.Producto)
       .ThenInclude(d => d.UnidadMedida)
       .Include(e => e.EntradaDetalles)
       .ThenInclude(d => d.Producto)
       .ThenInclude(d => d.Categoria)
       .Where(e => e.EntradaId > 0)
       .AsNoTracking()
       .ToListAsync();

        if (entradas == null) return new List<EntradaDetalle>();

        foreach (var item in entradas)
        {
            foreach (var p in item.EntradaDetalles)
            {
                if (p.Producto != null && p.FechaVencimiento <= DateTime.Now)
                {
                    entradasVencidas.Add(p);
                }
            }
        }

        return entradasVencidas;
    }

    public async Task<List<EntradaDetalle>> ListarEntradasVencidas(Expression<Func<EntradaDetalle, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradaDetalles
       .Include(d => d.Producto)
       .ThenInclude(d => d.UnidadMedida)
       .Include(d => d.Producto)
       .ThenInclude(d => d.Categoria)
       .Include(d => d. Entrada)
       .Where(criterio)
       .AsNoTracking()
       .ToListAsync();
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var entrada = await contexto.Entrada
            .Include(e => e.EntradaDetalles)
            .FirstOrDefaultAsync(e => e.EntradaId == id);

        if (entrada == null)
            return false;


        await AfectarExistencia(contexto, entrada.EntradaDetalles.ToArray(), false);


        entrada.Eliminado = true;
        contexto.Update(entrada);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Entrada>> ListarPapelera()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entrada
            .IgnoreQueryFilters()
            .Include(e => e.Proveedor)
            .Include(e => e.EntradaDetalles)
            .Where(e => e.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Restaurar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var entrada = await contexto.Entrada
            .IgnoreQueryFilters()
            .Include(e => e.EntradaDetalles)
            .FirstOrDefaultAsync(e => e.EntradaId == id);

        if (entrada == null)
            return false;


        await AfectarExistencia(contexto, entrada.EntradaDetalles.ToArray(), true);


        entrada.Eliminado = false;
        contexto.Entrada.Update(entrada);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> EliminarPermanente(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        return await contexto.Entrada
            .IgnoreQueryFilters()
            .Where(e => e.EntradaId == id)
            .ExecuteDeleteAsync() > 0;
    }
}