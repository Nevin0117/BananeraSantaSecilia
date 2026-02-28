using SantaSecilia.Domain.Entities;
using SantaSecilia.Application.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace SantaSecilia.ViewModels;

[QueryProperty(nameof(TrabajadorId), "TrabajadorId")]

public class EditarTrabajadorViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = null!){
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private readonly WorkerService _workerService;

    public ICommand GuardarCommand { get; }
    public ObservableCollection<string> Estados { get; } = new () { "Activo", "Inactivo" };


    string nombre;
    public string Nombre{
        get => nombre;
        set { nombre = value; OnPropertyChanged(); }
    }

    string cedula;
    public string Cedula{
        get => cedula;
        set { cedula = value; OnPropertyChanged(); }
    }

    bool activo;
    public bool Activo{
        get => activo;
        set { activo = value; OnPropertyChanged(); }
    }

    string estadoSeleccionado;
    public string EstadoSeleccionado
    {
        get => estadoSeleccionado;
        set
        {
            estadoSeleccionado = value;
            Activo = value == "Activo";
            OnPropertyChanged();
        }
    }

         int trabajadorId;
            public int TrabajadorId{
                get => trabajadorId;
                set{
                    trabajadorId = value;
                    _ = CargarTrabajador();
                }
            }

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
        EstadoSeleccionado = worker.IsActive ? "Activo" : "Inactivo";
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