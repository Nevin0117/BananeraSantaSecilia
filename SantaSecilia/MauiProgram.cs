using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SantaSecilia.Application.Services;
using SantaSecilia.Infrastructure.Data;
using SantaSecilia.Infrastructure.Repositories;
using SantaSecilia.ViewModels;
using SantaSecilia.Views;

namespace SantaSecilia;
public static class MauiProgram
{

    public static MauiApp CreateMauiApp()
    {
        // Asegura que SQLite esté inicializado en todas las plataformas (incluido mobile)
        SQLitePCL.Batteries_V2.Init();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddDbContextFactory<AppDbContext>((_, options) =>
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "santa_secilia.db3");
            options
                .UseSqlite($"Data Source={dbPath}")
                .AddInterceptors(new ForeignKeyInterceptor());
#if DEBUG
            options.EnableSensitiveDataLogging().EnableDetailedErrors();
#endif
        });

        builder.Services.AddScoped<LotRepository>();
        builder.Services.AddScoped<LotService>();
        builder.Services.AddScoped<LotesViewModel>();
        builder.Services.AddScoped<LotesPage>();
        builder.Services.AddScoped<RegistrarLotesViewModel>();

        

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Aplica migraciones al iniciar la aplicación (si las hay)
        ApplyMigrationsAsync(app.Services).GetAwaiter().GetResult();

        return app;
    }

    private static async Task ApplyMigrationsAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await using var db = await factory.CreateDbContextAsync();
        try
        {
            await db.Database.MigrateAsync();
        }
        catch
        {
            // Si ocurre un error durante la migración, intenta crear la base de datos desde cero
            await db.Database.EnsureCreatedAsync();
        }

    }

}
