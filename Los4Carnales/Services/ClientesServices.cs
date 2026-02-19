using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class ClientesServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
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
       
        if (cliente.ClienteId == 0)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var existeTelefono = await contexto.Cliente
                .AnyAsync(c => c.TelefonoCliente == cliente.TelefonoCliente);

            
        }

        if (!await Existe(cliente.ClienteId))
            return await Insertar(cliente);
        else
            return await Modificar(cliente);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cliente
            .Where(c => c.ClienteId == id)
            .ExecuteDeleteAsync() > 0;
    }

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