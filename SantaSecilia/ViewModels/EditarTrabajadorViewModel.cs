using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Data;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.Windows.Input;


namespace SantaSecilia.ViewModels;

[QueryProperty(nameof(Trabajador), "Trabajador")]
public class EditarTrabajadorViewModel
{
    private readonly AppDbContext _context;
    public ICommand GuardarCommand { get; }

    TrabajadorItem trabajador;
    public TrabajadorItem Trabajador
    {
        get => trabajador;
        set
        {
            trabajador = value;
            CargarDatos();
        }
    }

    public string Nombre { get; set; }
    public string Cedula { get; set; }
    public bool Activo { get; set; }
    public ObservableCollection<string> Estados { get; } =
        new ObservableCollection<string> { "Activo", "Inactivo" };

    public EditarTrabajadorViewModel(AppDbContext context)
    {
        _context = context;
        GuardarCommand = new Command(async () => await Guardar());
    }


    void CargarDatos()
    {
        if (Trabajador == null) return;

        Nombre = Trabajador.Nombre;
        Cedula = Trabajador.Cedula;
        Activo = Trabajador.Activo;
    }


    async Task Guardar()
    {
        if (Trabajador == null) return;

        var worker = await _context.Workers.FindAsync(Trabajador.Codigo);

        if (worker == null) return;

        worker.FullName = Nombre;
        worker.IdentificationNumber = Cedula;
        worker.IsActive = Activo;
        worker.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        await Shell.Current.GoToAsync("..");
    }
}