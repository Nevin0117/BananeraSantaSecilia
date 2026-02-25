using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SantaSecilia.Application.Services;
using SantaSecilia.Domain.Entities;
using System.Collections.ObjectModel;

namespace SantaSecilia.ViewModels;

[QueryProperty(nameof(ActividadId), "ActividadId")]
public partial class EditarActividadViewModel : ObservableObject
{
    private readonly ActivityService _activityService;

    [ObservableProperty]
    private int actividadId;

    [ObservableProperty]
    private string actividad = "";

    [ObservableProperty]
    private string tarifa = "";

    [ObservableProperty]
    private string estadoSeleccionado = "Activo";

    public ObservableCollection<string> EstadoOpciones { get; } =
        new ObservableCollection<string> { "Activo", "Inactivo" };

    public EditarActividadViewModel(ActivityService activityService)
    {
        _activityService = activityService;
    }

    partial void OnActividadIdChanged(int value)
    {
        _ = CargarActividadAsync(value);
    }

    private async Task CargarActividadAsync(int actividadId)
    {
        try
        {
            var actividades = await _activityService.ObtenerActividadesAsync();
            var actividad = actividades.FirstOrDefault(a => a.Id == actividadId);

            if (actividad != null)
            {
                Actividad = actividad.Name;
                Tarifa = actividad.HourlyRate.ToString();
                EstadoSeleccionado = actividad.IsActive ? "Activo" : "Inactivo";
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo cargar la actividad: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Actividad))
            {
                await Shell.Current.DisplayAlertAsync("Error", "El nombre de la actividad es requerido", "OK");
                return;
            }

            // Validar que la tarifa solo contenga números y punto decimal
            if (string.IsNullOrWhiteSpace(tarifa))
            {
                await Shell.Current.DisplayAlertAsync("Error", "La tarifa es requerida", "OK");
                return;
            }

            // Intentar convertir a decimal
            if (!decimal.TryParse(tarifa, out decimal tarifaDecimal))
            {
                await Shell.Current.DisplayAlertAsync("Error", "La tarifa debe ser un número válido", "OK");
                return;
            }

            if (tarifaDecimal <= 0)
            {
                await Shell.Current.DisplayAlertAsync("Error", "La tarifa debe ser mayor a 0", "OK");
                return;
            }

            if (tarifaDecimal > 99999)
            {
                await Shell.Current.DisplayAlertAsync("Error", "La tarifa no puede exceder 99,999", "OK");
                return;
            }

            if (await _activityService.ExisteActividadConNombreAsync(Actividad, ActividadId))
            {
                await Shell.Current.DisplayAlertAsync("Error", "Ya existe otra actividad con ese nombre", "OK");
                return;
            }

            var actividad = new Activity
            {
                Id = ActividadId,
                Name = Actividad,
                HourlyRate = tarifaDecimal,
                IsActive = EstadoSeleccionado == "Activo"
            };

            await _activityService.ActualizarActividadAsync(actividad);

            await Shell.Current.DisplayAlertAsync("Éxito", "Actividad actualizada correctamente", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo actualizar la actividad: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task CancelarAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}