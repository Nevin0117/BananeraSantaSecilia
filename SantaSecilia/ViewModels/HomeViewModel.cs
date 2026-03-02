using System.ComponentModel;
using SantaSecilia.Application.Services;

namespace SantaSecilia.ViewModels;

public class HomeViewModel : INotifyPropertyChanged
{
    private readonly HomeService _homeService;

    private int _totalJornaleros;
    private int _totalLotes;
    private int _registrosHoy;

    public int TotalJornaleros
    {
        get => _totalJornaleros;
        set { _totalJornaleros = value; OnPropertyChanged(nameof(TotalJornaleros)); }
    }

    public int TotalLotes
    {
        get => _totalLotes;
        set { _totalLotes = value; OnPropertyChanged(nameof(TotalLotes)); }
    }

    public int RegistrosHoy
    {
        get => _registrosHoy;
        set { _registrosHoy = value; OnPropertyChanged(nameof(RegistrosHoy)); }
    }

    public HomeViewModel(HomeService homeService)
    {
        _homeService = homeService;
    }

    public async Task CargarResumenAsync()
    {
        try
        {
            var (jornaleros, lotes, registros) = await _homeService.ObtenerResumenHoyAsync();

            TotalJornaleros = jornaleros;
            TotalLotes = lotes;
            RegistrosHoy = registros;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar el resumen del Home: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}