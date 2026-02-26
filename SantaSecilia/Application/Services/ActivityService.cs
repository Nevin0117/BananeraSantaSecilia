using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Repositories;

namespace SantaSecilia.Application.Services;

public class ActivityService
{
    private readonly ActivityRepository _repository;

    public ActivityService(ActivityRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Activity>> ObtenerActividadesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<Activity>> ObtenerActividadesActivasAsync()
    {
        return await _repository.GetActiveAsync();
    }

    public async Task CrearActividadAsync(string nombre, decimal tarifaPorHora, bool activa = true)
    {
        var actividad = new Activity
        {
            Name = nombre,
            HourlyRate = tarifaPorHora,
            IsActive = activa,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(actividad);
    }

    public async Task ActualizarActividadAsync(Activity actividad)
    {
        actividad.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(actividad);
    }

    public async Task DesactivarActividadAsync(int id)
    {
        var actividad = await _repository.GetByIdAsync(id);
        if (actividad != null && actividad.IsActive)
        {
            actividad.IsActive = false;
            actividad.DeactivatedAt = DateTime.UtcNow;
            actividad.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(actividad);
        }
    }

    public async Task<bool> ExisteActividadConNombreAsync(string nombre, int? ignorarId = null)
    {
        return await _repository.ExistsByNameAsync(nombre, ignorarId);
    }
}