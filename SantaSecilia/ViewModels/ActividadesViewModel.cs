using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using SantaSecilia.Views;
using SantaSecilia.Application.Services;
using SantaSecilia.Domain.Entities;
using System.Windows.Input;

namespace SantaSecilia.ViewModels;

public class ActividadItem
{
    public int Id { get; set; }
    public required string Actividad { get; set; }
    public required decimal Tarifa { get; set; }
    public string Estado => Activo ? "Activo" : "Inactivo";
    public bool Activo { get; set; }
}

public class ActividadesViewModel
{
    private readonly ActivityService _activityService;

    public ObservableCollection<ActividadItem> Actividades { get; set; }
    public ICommand RegistrarCommand { get; }
    public ICommand EditarCommand { get; }

    public ActividadesViewModel(ActivityService activityService)
    {
        _activityService = activityService;
        Actividades = new ObservableCollection<ActividadItem>();

        RegistrarCommand = new Command(async () =>
            await Shell.Current.GoToAsync(nameof(RegistrarActividadPage)));

        EditarCommand = new Command<ActividadItem>(async (actividad) =>
        {
            // Pasar el ID como parámetro en la navegación
            await Shell.Current.GoToAsync($"{nameof(EditarActividadPage)}?ActividadId={actividad.Id}");
        });

        _ = CargarActividadesAsync();
    }

    public async Task CargarActividadesAsync()
    {
        try
        {
            var actividades = await _activityService.ObtenerActividadesAsync();

            Actividades.Clear();
            foreach (var actividad in actividades)
            {
                Actividades.Add(new ActividadItem
                {
                    Id = actividad.Id,
                    Actividad = actividad.Name,
                    Tarifa = actividad.HourlyRate,
                    Activo = actividad.IsActive
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error cargando actividades: {ex.Message}");
        }
    }
}