using Los4Carnales.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ProyectoFinalAplicada1;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        // 1. Obtener los servicios necesarios
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // 2. Definir los roles que quieres crear
        string[] roleNames = { "Administrador", "Master" };

        // Crear los roles si no existen
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 3. Crear un usuario Administrador (si no existe)
        const string adminEmail = "admin@gmail.com"; // Usar minúsculas es más seguro
        const string adminPassword = "@Clave123";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,

                // 🔑 INICIALIZACIÓN DE CAMPOS REQUERIDOS DE CLIENTE/USUARIO:
                // Si ApplicationUser heredó o implementó campos [Required] de tu modelo Cliente,
                // DEBEN inicializarse aquí para que CreateAsync no falle.
                Nombre = "Administrador",
                Apellido = "Principal",
                Telefono = "8090000000",
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                // 4. Asignar el rol de "Administrador"
                await userManager.AddToRoleAsync(adminUser, "Administrador");
            }
            else
            {
                // 🚨 CÓDIGO DE MANEJO DE ERRORES: Muestra la causa exacta del fallo 🚨
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                throw new InvalidOperationException($"El seeding del Admin falló: {errors} Verifique los requisitos...");
            }
        }
    }
}