using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SantaSecilia.Application.Services;
using System.Collections.ObjectModel;

namespace SantaSecilia.ViewModels;

public partial class RegistrarActividadViewModel : ObservableObject
{
    private readonly ActivityService _activityService;

    [ObservableProperty]
    private string actividad = "";

    [ObservableProperty]
    private string tarifa = "";

    [ObservableProperty]
    private string estadoSeleccionado = "Activo";

    public ObservableCollection<string> EstadoOpciones { get; } =
        new ObservableCollection<string> { "Activo", "Inactivo" };

    public RegistrarActividadViewModel(ActivityService activityService)
    {
        _activityService = activityService;
    }

    public void LimpiarCampos()
    {
        Actividad = "";
        Tarifa = "";
        EstadoSeleccionado = "Activo";
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

            if (string.IsNullOrWhiteSpace(tarifa))
            {
                await Shell.Current.DisplayAlertAsync("Error", "La tarifa es requerida", "OK");
                return;
            }

           
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

            if (await _activityService.ExisteActividadConNombreAsync(Actividad))
            {
                await Shell.Current.DisplayAlertAsync("Error", "Ya existe una actividad con ese nombre", "OK");
                return;
            }

            bool activa = EstadoSeleccionado == "Activo";
            await _activityService.CrearActividadAsync(Actividad, tarifaDecimal, activa);

            await Shell.Current.DisplayAlertAsync("Éxito", "Actividad registrada correctamente", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo guardar: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task CancelarAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}