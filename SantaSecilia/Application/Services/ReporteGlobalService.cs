using Microsoft.EntityFrameworkCore;
using SantaSecilia.Application.DTOs;
using SantaSecilia.Infrastructure.Data;

namespace SantaSecilia.Application.Services;

public class ReporteGlobalService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ReporteGlobalService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public (DateTime lunes, DateTime viernes) ObtenerSemanaLaboral(DateTime fecha)
    {
        // Calcular el lunes de la semana
        int diff = (7 + (fecha.DayOfWeek - DayOfWeek.Monday)) % 7;
        DateTime lunes = fecha.AddDays(-diff).Date;

        // El viernes es 4 días después del lunes
        DateTime viernes = lunes.AddDays(4).Date;

        return (lunes, viernes);
    }

    public async Task<ReporteGlobalDto> GenerarReporteAsync(DateTime fechaReferencia)
    {
        var (lunes, viernes) = ObtenerSemanaLaboral(fechaReferencia);

        await using var context = await _contextFactory.CreateDbContextAsync();

        // Consultar todos los registros de la semana con sus relaciones
        var registrosSemana = await context.Set<Domain.Entities.DailyRecordLine>()
            .Include(drl => drl.Activity)
            .Include(drl => drl.DailyRecord)
            .Where(drl => drl.DailyRecord.WorkDate >= lunes && drl.DailyRecord.WorkDate <= viernes)
            .ToListAsync();

        // Contar trabajadores únicos que trabajaron en la semana
        var totalJornaleros = registrosSemana
            .Select(drl => drl.DailyRecord.WorkerId)
            .Distinct()
            .Count();

        // Agrupar por actividad y sumar horas
        var actividadesAgrupadas = registrosSemana
            .GroupBy(drl => new
            {
                ActividadId = drl.Activity.Id,
                ActividadNombre = drl.Activity.Name,
                TarifaPorHora = drl.Activity.HourlyRate
            })
            .Select(grupo => new ActividadReporteDto
            {
                NombreActividad = grupo.Key.ActividadNombre,
                HorasTotales = grupo.Sum(drl => drl.Hours),
                TarifaPorHora = grupo.Key.TarifaPorHora,
                TotalPorActividad = grupo.Sum(drl => drl.Hours) * grupo.Key.TarifaPorHora * 100
            })
            .OrderBy(a => a.NombreActividad)
            .ToList();

        var totalPagado = actividadesAgrupadas.Sum(a => a.TotalPorActividad);

        return new ReporteGlobalDto
        {
            FechaInicio = lunes,
            FechaFin = viernes,
            Actividades = actividadesAgrupadas,
            TotalPagado = totalPagado,
            TotalJornaleros = totalJornaleros
        };
    }
}