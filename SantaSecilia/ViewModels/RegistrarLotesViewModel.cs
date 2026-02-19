using System.Collections.ObjectModel;
using System.Windows.Input;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Repositories;

namespace SantaSecilia.ViewModels;

public class RegistrarLotesViewModel
{
    private readonly LotRepository _repository;

    public string Codigo { get; set; }

    public ObservableCollection<string> Estados { get; } =
        new ObservableCollection<string> { "Activo", "Inactivo" };

    public string EstadoSeleccionado { get; set; } = "Activo";

    public ICommand RegistrarCommand { get; }
    public ICommand CancelarCommand { get; }

    public RegistrarLotesViewModel(LotRepository repository)
    {
        _repository = repository;

        RegistrarCommand = new Command(async () => await RegistrarAsync());
        CancelarCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    private async Task RegistrarAsync()
    {
        // 1️⃣ Validar que no esté vacío
        if (string.IsNullOrWhiteSpace(Codigo))
        {
            await Shell.Current.DisplayAlertAsync("Error",
                "Debe ingresar un código.",
                "OK");
            return;
        }

        // 2️⃣ Validar que sea número válido
        if (!int.TryParse(Codigo, out int codigoNumerico))
        {
            await Shell.Current.DisplayAlertAsync("Error",
                "El código debe ser numérico.",
                "OK");
            return;
        }

        // 3️⃣ Validar que no exista ya
        if (await _repository.ExistsByCodeAsync(codigoNumerico))
        {
            await Shell.Current.DisplayAlertAsync("Error",
                "Ya existe un lote con ese código.",
                "OK");
            return;
        }

        // 4️⃣ Crear lote
        var nuevoLote = new Lot
        {
            Code = codigoNumerico,
            IsActive = EstadoSeleccionado == "Activo",
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(nuevoLote);

        await Shell.Current.GoToAsync("..");
    }
}

