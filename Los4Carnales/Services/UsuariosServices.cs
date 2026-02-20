using Los4Carnales.Data;
using Los4Carnales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Los4Carnales.Services;

public class UsuarioServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int idUsuario)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Usuario.AnyAsync(c => c.UsuarioId == idUsuario);
    }

    public async Task<Usuario?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        return await contexto.Usuario.FirstOrDefaultAsync(e => e.UsuarioId == id);
    }

    public async Task<List<Usuario>> Listar(Expression<Func<Usuario, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Usuario.Where(criterio).AsNoTracking().ToListAsync();

    }

    public async Task<bool> Insertar(Usuario usuario)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Usuario.Add(usuario);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Usuario usuario)
    {
        if (!await Existe(usuario.UsuarioId))
        {

            return await Insertar(usuario);
        }
        else
        {
            return await Modificar(usuario);
        }
    }

    public async Task<bool> Modificar(Usuario usuario)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(usuario);
        return await contexto.SaveChangesAsync() > 0;
    }

}
