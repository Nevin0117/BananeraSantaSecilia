using Microsoft.EntityFrameworkCore;
using SantaSecilia.Infrastructure.Data;

namespace SantaSecilia.Application.Services;

public class HomeService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public HomeService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<(int jornaleros, int lotes, int registrosHoy)> ObtenerResumenHoyAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var hoy = DateTime.Today;

        // Jornaleros Activos 
        var totalJornaleros = await context.Set<Domain.Entities.Worker>()
            .CountAsync(w => w.IsActive);

        // Lotes Activos
        var totalLotes = await context.Set<Domain.Entities.Lot>()
            .CountAsync(l => l.IsActive);

        // Accedemos a la fecha a través de la propiedad de navegación DailyRecord
        var registrosHoy = await context.Set<Domain.Entities.DailyRecordLine>()
            .CountAsync(drl => drl.DailyRecord.WorkDate >= hoy && drl.DailyRecord.WorkDate < hoy.AddDays(1));

        return (totalJornaleros, totalLotes, registrosHoy);
    }
}