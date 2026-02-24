using System.Collections.ObjectModel;
using System.Windows.Input;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Repositories;
using SantaSecilia.Views;

namespace SantaSecilia.ViewModels;

public class LotesViewModel
{
    private readonly LotRepository _repository;

    public ObservableCollection<Lot> Lotes { get; set; } = new();

    public ICommand RegistrarCommand { get; }
    public ICommand EditarCommand { get; }
    public ICommand EliminarCommand { get; }

    // Inicializa el ViewModel y configura los comandos para registrar, editar y eliminar lotes
    public LotesViewModel(LotRepository repository)
    {
        _repository = repository;

        // REGISTRAR LOTE
        RegistrarCommand = new Command(async () =>
            await Shell.Current.GoToAsync(nameof(RegistrarLotesPage)));

        // EDITAR LOTE
        EditarCommand = new Command<Lot>(async (lot) =>
        {
            if (lot == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(EditarLotesPage)}?id={lot.Id}");
        });

        // ELIMINAR LOTE
        EliminarCommand = new Command<Lot>(async (lot) => 
            await EliminarAsync(lot));

    }
    // Cargar todos los lotes desde la BD y actualizar la colección observable
    public async Task LoadAsync()
    {
        var lots = await _repository.GetAllAsync();

        Lotes.Clear();

        foreach (var lot in lots)
            Lotes.Add(lot);
    }

    // Solicitar confirmación y eliminar lote seleccionado
    private async Task EliminarAsync(Lot lote)
    {
        if (lote == null)
            return;

        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Confirmar",
            $"¿Desea eliminar el lote {lote.Code}?",
            "Sí",
            "No");

        if (!confirm)
            return;

        await _repository.DeleteAsync(lote.Id);

        await LoadAsync();
    }


}
