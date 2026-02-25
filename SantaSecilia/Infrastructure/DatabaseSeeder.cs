using Microsoft.EntityFrameworkCore;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Data;
using System.Security.Cryptography;

namespace SantaSecilia.Infrastructure;

public static class DatabaseSeeder
{
    private static readonly Dictionary<string, decimal> ActividadesTarifas = new()
    {
        { "Salario mínimo convencional", 0.7011m },
        { "Hacer puente para conchero", 0.7790m },
        { "Regar herbicida", 0.7790m },
        { "Chequear nemátodos", 0.7790m },
        { "Desinfectar herramientas", 0.7790m },
        { "Fumigar bolsas", 0.7790m },
        { "Limpiar empacadora", 0.7790m },
        { "Botar pizote", 0.7790m },
        { "Banderero", 0.7790m },
        { "Nivelar caminos", 0.8012m },
        { "Ayudante mecánico", 0.8012m },
        { "Ayudante soldador", 0.8012m },
        { "Celador", 0.8123m },
        { "Sanidad (fin de vivienda/baños)", 0.8123m },
        { "Mecánico", 1.0126m },
        { "Soldador", 1.0126m },
        { "Carpintero", 1.0126m },
        { "Ayudante irrigación", 0.8266m },
        { "Cedaceros", 0.7053m },
        { "Reapuntalar bananal", 0.7011m },
        { "Sembrar leguminosas/gramíneas", 0.7715m },
        { "Control de Sigatoka (deshoje)", 0.9368m },
        { "Desviar banderilla", 0.7164m },
        { "Sacar matas hospederas", 0.7935m },
        { "Mantenimiento hierbas canales", 0.7715m },
        { "Aporcar semilleros", 0.7164m },
        { "Tiburón (herramienta)", 0.7584m },
        { "Mantenimiento de plantillo", 0.9042m },
        { "Mantenimiento de semillero", 0.8955m },
        { "Apuntalar con bambú", 0.7164m },
        { "Corregir población", 0.8955m },
        { "Desviar fruta orilla caminos", 0.7011m },
        { "Cargar bambú", 0.7011m },
        { "Limpieza de boquetes", 0.7011m }
    };

    public static async Task SeedUsersAsync(IServiceProvider services)
    {
        var factory = services.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await using var context = await factory.CreateDbContextAsync();

        if (context.Users.Any())
            return;

        var adminPassword = HashPassword("admin123");
        var user = new User
        {
            Username = "admin",
            Email = "admin@santasecilia.com",
            FullName = "Administrador",
            PasswordHash = adminPassword,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public static async Task SeedActivitiesAsync(IServiceProvider services)
    {
        var factory = services.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await using var context = await factory.CreateDbContextAsync();

        if (context.Activities.Any())
            return;

        var now = DateTime.UtcNow;
        foreach (var (name, rate) in ActividadesTarifas)
        {
            context.Activities.Add(new Activity
            {
                Name = name,
                HourlyRate = rate,
                IsActive = true,
                CreatedAt = now
            });
        }

        await context.SaveChangesAsync();
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations: 100000,
            hashAlgorithm: System.Security.Cryptography.HashAlgorithmName.SHA256,
            outputLength: 32);

        return $"{Convert.ToHexString(salt)}:{Convert.ToHexString(hash)}";
    }
}
