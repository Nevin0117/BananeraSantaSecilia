using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Repositories;

namespace SantaSecilia.Application.Services;

public class DailyRecordService(
    DailyRecordRepository recordRepository,
    ActivityRepository activityRepository)
{
    private readonly DailyRecordRepository _recordRepository = recordRepository;
    private readonly ActivityRepository _activityRepository = activityRepository;

    public async Task<(bool Success, string Message)> AddWorkLineAsync(
        int workerId,
        DateTime date,
        int activityId,
        int lotId,
        int hours)
    {
        // 1. Validar que la actividad existe
        var activity = await _activityRepository.GetByIdAsync(activityId);
        if (activity == null) return (false, "La actividad seleccionada no existe.");

        // 2. Obtener o crear el encabezado del registro diario
        var record = await _recordRepository.GetByWorkerAndDateAsync(workerId, date);
        bool isNewRecord = false;

        if (record == null)
        {
            record = new DailyRecord
            {
                WorkerId = workerId,
                WorkDate = date.Date,
                CreatedAt = DateTime.UtcNow,
                Lines = new List<DailyRecordLine>()
            };
            isNewRecord = true;
        }

        // 3. Validar límite de horas (máximo 24h por día)
        int currentTotalHours = record.Lines.Sum(l => l.Hours);
        if (currentTotalHours + hours > 24)
        {
            return (false, $"El trabajador ya tiene {currentTotalHours}h registradas. No puede exceder las 24h diarias.");
        }

        // 4. Crear la línea con el Snapshot de la tarifa actual
        var newLine = new DailyRecordLine
        {
            ActivityId = activityId,
            LotId = lotId,
            Hours = hours,
            RateSnapshot = activity.HourlyRate,
            SubtotalSnapshot = activity.HourlyRate * hours
        };

        if (isNewRecord)
        {
            record.Lines.Add(newLine);
            await _recordRepository.AddAsync(record);
        }
        else
        {
            newLine.DailyRecordId = record.Id;
            await _recordRepository.SaveLineAsync(newLine);
        }

        return (true, "Registro guardado exitosamente.");
    }

    public async Task<DailyRecord?> GetRecordAsync(int workerId, DateTime date)
    {
        return await _recordRepository.GetByWorkerAndDateAsync(workerId, date);
    }
}
