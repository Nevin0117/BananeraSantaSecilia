using System.Collections.ObjectModel;
using System.ComponentModel;
using SantaSecilia.Application.Services;
using SantaSecilia.Application.DTOs;

namespace SantaSecilia.ViewModels;

public class ActividadReporteItem
{
    public string Actividad { get; set; } = "";
    public string Horas { get; set; } = "";
    public string Tarifa { get; set; } = "";
    public string Total { get; set; } = "";
}

public class ReporteGlobalViewModel : INotifyPropertyChanged
{
    private readonly ReporteGlobalService _reporteService;
    private DateTime _fechaSeleccionada = DateTime.Today;
    private string _totalPagado = "—";
    private bool _hayDatos = false;

    public ObservableCollection<ActividadReporteItem> Actividades { get; set; }

    public DateTime FechaSeleccionada
    {
        get => _fechaSeleccionada;
        set
        {
            if (_fechaSeleccionada != value)
            {
                _fechaSeleccionada = value;
                OnPropertyChanged(nameof(FechaSeleccionada));
                _ = GenerarReporteAsync();
            }
        }
    }

    public string TotalPagado
    {
        get => _totalPagado;
        set
        {
            if (_totalPagado != value)
            {
                _totalPagado = value;
                OnPropertyChanged(nameof(TotalPagado));
            }
        }
    }

    public bool HayDatos
    {
        get => _hayDatos;
        set
        {
            if (_hayDatos != value)
            {
                _hayDatos = value;
                OnPropertyChanged(nameof(HayDatos));
                OnPropertyChanged(nameof(MostrarMensajeVacio));
            }
        }
    }

    public bool MostrarMensajeVacio => !HayDatos;

    public ReporteGlobalViewModel(ReporteGlobalService reporteService)
    {
        _reporteService = reporteService;
        Actividades = new ObservableCollection<ActividadReporteItem>();
    }

    public async Task GenerarReporteAsync()
    {
        try
        {
            var reporte = await _reporteService.GenerarReporteAsync(FechaSeleccionada);

            Actividades.Clear();

            foreach (var actividad in reporte.Actividades)
            {
                Actividades.Add(new ActividadReporteItem
                {
                    Actividad = actividad.NombreActividad,
                    Horas = actividad.HorasTotales.ToString("F2"),
                    Tarifa = actividad.TarifaPorHora.ToString("F4"),
                    Total = $"B/. {actividad.TotalPorActividad:N2}"
                });
            }

            TotalPagado = reporte.Actividades.Any()
                ? $"B/. {reporte.TotalPagado:N2}"
                : "—";

            HayDatos = reporte.Actividades.Any();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error generando reporte: {ex.Message}");
            HayDatos = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}