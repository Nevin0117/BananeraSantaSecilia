using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SantaSecilia.Application.Services;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Repositories;
using System.Collections.ObjectModel;

namespace SantaSecilia.ViewModels;

public partial class RegistroLaborViewModel : ObservableObject
{
    private readonly DailyRecordService _recordService;
    private readonly WorkerService _workerService;
    private readonly LotRepository _lotRepository;
    private readonly ActivityRepository _activityRepository;

    public RegistroLaborViewModel(
        DailyRecordService recordService,
        WorkerService workerService,
        LotRepository lotRepository,
        ActivityRepository activityRepository)
    {
        _recordService = recordService;
        _workerService = workerService;
        _lotRepository = lotRepository;
        _activityRepository = activityRepository;
    }

    [ObservableProperty]
    private ObservableCollection<Activity> _actividades = new();

    [ObservableProperty]
    private ObservableCollection<Lot> _lotes = new();

    [ObservableProperty]
    private ObservableCollection<Worker> _trabajadoresSugeridos = new();

    [ObservableProperty]
    private string _trabajadorBusqueda = "";

    [ObservableProperty]
    private Worker? _selectedWorker;

    [ObservableProperty]
    private Activity? _selectedActivity;

    [ObservableProperty]
    private Lot? _selectedLot;

    [ObservableProperty]
    private int _horasTrabajadas = 0;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _errorMessage = "";

    [ObservableProperty]
    private bool _isSuccess;

    public string MontoAutogenerado => (HorasTrabajadas * 100 * (SelectedActivity?.HourlyRate ?? 0)).ToString("C2");

    public async Task InitializeAsync()
    {
        IsBusy = true;
        try
        {
            var activities = await _activityRepository.GetActiveAsync();
            var lots = await _lotRepository.GetActiveAsync();

            Actividades = new ObservableCollection<Activity>(activities);
            Lotes = new ObservableCollection<Lot>(lots);
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnTrabajadorBusquedaChanged(string value)
    {
        if (value.Length >= 2)
            _ = SearchWorkersAsync(value);
        else
            TrabajadoresSugeridos.Clear();
    }

    private async Task SearchWorkersAsync(string query)
    {
        var results = await _workerService.BuscarTrabajadoresAsync(query);
        TrabajadoresSugeridos = new ObservableCollection<Worker>(results);
    }

    [RelayCommand]
    private void SelectWorker(Worker worker)
    {
        SelectedWorker = worker;
        TrabajadorBusqueda = worker.DisplayInfo;
        TrabajadoresSugeridos.Clear();
    }

    partial void OnHorasTrabajadasChanged(int value) => OnPropertyChanged(nameof(MontoAutogenerado));
    partial void OnSelectedActivityChanged(Activity? value) => OnPropertyChanged(nameof(MontoAutogenerado));

    [RelayCommand]
    private async Task GuardarRegistroAsync()
    {
        if (IsBusy || SelectedWorker == null || SelectedActivity == null || SelectedLot == null)
        {
            ErrorMessage = "Por favor complete todos los campos requeridos.";
            return;
        }

        if (HorasTrabajadas <= 0)
        {
            ErrorMessage = "Las horas deben ser mayores a cero.";
            return;
        }

        IsBusy = true;
        ErrorMessage = "";
        IsSuccess = false;

        try
        {
            var result = await _recordService.AddWorkLineAsync(
                SelectedWorker.Id,
                DateTime.Today,
                SelectedActivity.Id,
                SelectedLot.Id,
                HorasTrabajadas);

            if (result.Success)
            {
                IsSuccess = true;
                ErrorMessage = result.Message;
                ResetForm();

                IsBusy = false;
                // Esperamos 3 segundos en segundo plano
                await Task.Delay(3000);

                // Limpiamos los estados para que el mensaje desaparezca de la UI
                IsSuccess = false;
                ErrorMessage = "";
            }
            else
            {
                ErrorMessage = result.Message;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ResetForm()
    {
        TrabajadorBusqueda = "";
        SelectedWorker = null;
        SelectedActivity = null;
        SelectedLot = null;
        HorasTrabajadas = 0;
    }

    [RelayCommand]
    private async Task CancelarAsync()
    {
        await Shell.Current.GoToAsync("//Home");
    }
}
