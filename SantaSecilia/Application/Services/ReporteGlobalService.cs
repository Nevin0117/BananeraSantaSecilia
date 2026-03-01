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

    public (DateTime domingo, DateTime sabado) ObtenerSemanaLaboral(DateTime fecha)
    {
        // Calculamos el domingo de la semana (DayOfWeek.Sunday es 0 en .NET)
        int diff = (int)fecha.DayOfWeek;
        DateTime domingo = fecha.AddDays(-diff).Date;

        // El sábado es 6 días después del domingo
        DateTime sabado = domingo.AddDays(6).Date;

        return (domingo, sabado);
    }

    public async Task<ReporteGlobalDto> GenerarReporteAsync(DateTime fechaReferencia)
    {
        var (inicio, fin) = ObtenerSemanaLaboral(fechaReferencia);

        await using var context = await _contextFactory.CreateDbContextAsync();

        // Consultar todos los registros de la semana con sus relaciones
        var registrosSemana = await context.Set<Domain.Entities.DailyRecordLine>()
            .Include(drl => drl.Activity)
            .Include(drl => drl.DailyRecord)
            // Usamos el nuevo rango completo
            .Where(drl => drl.DailyRecord.WorkDate >= inicio && drl.DailyRecord.WorkDate <= fin)
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
            FechaInicio = inicio,
            FechaFin = fin,
            Actividades = actividadesAgrupadas,
            TotalPagado = totalPagado,
            TotalJornaleros = totalJornaleros
        };
    }
}