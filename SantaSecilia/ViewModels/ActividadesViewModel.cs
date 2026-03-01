using System.Collections.ObjectModel;
using System.ComponentModel;
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

public class ActividadesViewModel : INotifyPropertyChanged
{
    private readonly ActivityService _activityService;
    private List<ActividadItem> _todasLasActividades = new(); // Lista completa sin filtrar
    private string _textoBusqueda = "";
    public bool IsSearching => !string.IsNullOrWhiteSpace(TextoBusqueda);

    public ObservableCollection<ActividadItem> Actividades { get; set; }

    public string TextoBusqueda
    {
        get => _textoBusqueda;
        set
        {
            if (_textoBusqueda != value)
            {
                _textoBusqueda = value;
                OnPropertyChanged(nameof(TextoBusqueda));

                OnPropertyChanged(nameof(IsSearching));

                FiltrarActividades();
            }
        }
    }

    public ICommand RegistrarCommand { get; }
    public ICommand EditarCommand { get; }
    public ICommand EliminarCommand { get; }
    public ICommand LimpiarBusquedaCommand { get; }

    public ActividadesViewModel(ActivityService activityService)
    {
        _activityService = activityService;
        Actividades = new ObservableCollection<ActividadItem>();

        RegistrarCommand = new Command(async () =>
            await Shell.Current.GoToAsync(nameof(RegistrarActividadPage)));

        EditarCommand = new Command<ActividadItem>(async (actividad) =>
        {
            await Shell.Current.GoToAsync($"{nameof(EditarActividadPage)}?ActividadId={actividad.Id}");
        });

        EliminarCommand = new Command<ActividadItem>(async (actividad) =>
        {
            await EliminarActividadAsync(actividad);
        });

        LimpiarBusquedaCommand = new Command(() =>
        {
            TextoBusqueda = string.Empty;
        });

        _ = CargarActividadesAsync();
    }

    private void FiltrarActividades()
    {
        Actividades.Clear();

        var actividadesFiltradas = string.IsNullOrWhiteSpace(TextoBusqueda)
            ? _todasLasActividades
            : _todasLasActividades.Where(a =>
                a.Actividad.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                a.Tarifa.ToString().Contains(TextoBusqueda) ||
                a.Estado.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase)
              ).ToList();

        foreach (var actividad in actividadesFiltradas)
        {
            Actividades.Add(actividad);
        }
    }

    private async Task EliminarActividadAsync(ActividadItem actividad)
    {
        try
        {
            bool confirmar = await Shell.Current.DisplayAlertAsync(
                "Confirmar eliminación",
                $"¿Está seguro de eliminar permanentemente la actividad '{actividad.Actividad}'?",
                "Eliminar",
                "Cancelar");

            if (!confirmar)
                return;

            var (success, message) = await _activityService.EliminarActividadAsync(actividad.Id);

            if (success)
            {
                await Shell.Current.DisplayAlertAsync("Éxito", message, "OK");
                await CargarActividadesAsync();
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Información", message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Error inesperado: {ex.Message}", "OK");
        }
    }

    public async Task CargarActividadesAsync()
    {
        try
        {
            var actividades = await _activityService.ObtenerActividadesAsync();

            _todasLasActividades.Clear();
            foreach (var actividad in actividades)
            {
                _todasLasActividades.Add(new ActividadItem
                {
                    Id = actividad.Id,
                    Actividad = actividad.Name,
                    Tarifa = actividad.HourlyRate,
                    Activo = actividad.IsActive
                });
            }

            FiltrarActividades(); // Aplicar filtro o mostrar todo
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error cargando actividades: {ex.Message}");
        }
    }

    // Implementación de INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}