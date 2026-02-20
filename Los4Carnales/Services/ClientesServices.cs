using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Los4Carnales.Data;
using Los4Carnales.Models;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class ClientesServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cliente.AnyAsync(c => c.ClienteId == id);
    }

    private async Task<bool> Insertar(Cliente cliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Cliente.Add(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Cliente cliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Cliente cliente)
    {
        if (!await Existe(cliente.ClienteId))
            return await Insertar(cliente);
        else
            return await Modificar(cliente);
    }

    // --- NUEVA LÓGICA DE BORRADO LÓGICO ---

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var cliente = await contexto.Cliente.FindAsync(id);
        if (cliente == null) return false;

        cliente.Eliminado = true; // Asumiendo que la propiedad existe en el modelo Cliente
        contexto.Update(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    // Listar clientes que están en la papelera
    public async Task<List<Cliente>> ListarPapelera()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cliente
            .IgnoreQueryFilters()
            .Where(c => c.Eliminado)
            .AsNoTracking()
            .ToListAsync();
    }

    // Restaurar un cliente de la papelera
    public async Task<bool> Restaurar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var cliente = await contexto.Cliente
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClienteId == id);

        if (cliente == null) return false;

        cliente.Eliminado = false;
        contexto.Update(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    // Borrado físico definitivo
    public async Task<bool> EliminarPermanente(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cliente
            .IgnoreQueryFilters()
            .Where(c => c.ClienteId == id)
            .ExecuteDeleteAsync() > 0;
    }

    // --- BÚSQUEDAS Y LISTADOS ---

    public async Task<Cliente?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cliente
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ClienteId == id);
    }

    public async Task<List<Cliente>> Listar(Expression<Func<Cliente, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cliente
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Cliente?> BuscarPorTelefono(string telefono)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cliente
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.TelefonoCliente == telefono);
    }

    public async Task<Cliente?> BuscarPorNombre(string nombre)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cliente
            .FirstOrDefaultAsync(c => c.NombreCliente == nombre || c.DescripcionCliente.Contains(nombre));
    }
}