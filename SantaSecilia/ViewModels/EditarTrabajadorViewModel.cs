using SantaSecilia.Domain.Entities;
using SantaSecilia.Application.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace SantaSecilia.ViewModels;

[QueryProperty(nameof(TrabajadorId), "TrabajadorId")]
public class EditarTrabajadorViewModel
{
    private readonly WorkerService _workerService;

    public ICommand GuardarCommand { get; }

    public required string Nombre { get; set; }
    public required string Cedula { get; set; }
    public bool Activo { get; set; }

    int trabajadorId;
    public int TrabajadorId
    {
        get => trabajadorId;
        set
        {
            trabajadorId = value;
            _ = CargarTrabajador();
        }
    }

    public ObservableCollection<string> Estados { get; } = new ObservableCollection<string> { "Activo", "Inactivo" };

    public EditarTrabajadorViewModel(WorkerService workerService)
    {
        _workerService = workerService;

        GuardarCommand = new Command(async () => await Guardar());
    }

    async Task CargarTrabajador()
    {
        var worker = await _workerService.ObtenerPorIdAsync(TrabajadorId);

        if (worker == null) return;

        Nombre = worker.FullName;
        Cedula = worker.IdentificationNumber;
        Activo = worker.IsActive;
    }

    async Task Guardar()
    {
        var worker = new Worker
        {
            Id = TrabajadorId,
            FullName = Nombre,
            IdentificationNumber = Cedula,
            IsActive = Activo
        };

        await _workerService.ActualizarTrabajadorAsync(worker);

        await Shell.Current.GoToAsync("..");
    }
}