using Los4Carnales.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Los4Carnales;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // 1. Definir y crear roles
        string[] roleNames = { "Administrador", "Master" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 2. Crear cuenta Administrador
        await CreateUserIfNotExist(userManager,
            "admin@gmail.com",
            "@Clave123",
            "Administrador",
            "Principal",
            "Administrador");

        // 3. Crear cuenta Master 🔑
        await CreateUserIfNotExist(userManager,
            "master@gmail.com",
            "@ClaveMaster123",
            "Master",
            "Sistema",
            "Master");
    }

    private static async Task CreateUserIfNotExist(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string nombre,
        string apellido,
        string role)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Nombre = nombre,
                Apellido = apellido,
                Telefono = "8090000000" // Valor por defecto o parámetro adicional
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Fallo al crear el usuario {email}: {errors}");
            }
        }
    }
}