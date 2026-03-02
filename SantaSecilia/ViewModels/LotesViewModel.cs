using SantaSecilia.Application.Services;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SantaSecilia.ViewModels;

public class LotesViewModel
{
    private readonly LotService _lotService; // Ahora usamos el servicio

    public ObservableCollection<Lot> Lotes { get; set; } = new();

    public ICommand RegistrarCommand { get; }
    public ICommand EditarCommand { get; }
    public ICommand EliminarCommand { get; }

    public LotesViewModel(LotService lotService)
    {
        _lotService = lotService;

        RegistrarCommand = new Command(async () =>
            await Shell.Current.GoToAsync(nameof(RegistrarLotesPage)));

        EditarCommand = new Command<Lot>(async (lot) =>
        {
            if (lot == null) return;
            await Shell.Current.GoToAsync($"{nameof(EditarLotesPage)}?id={lot.Id}");
        });

        EliminarCommand = new Command<Lot>(async (lot) =>
        {
            await EliminarAsync(lot);
        });
    }

    // CORRECCIÓN 1: Usar _lotService en lugar de _repository
    public async Task LoadAsync()
    {
        // Llamamos al método del servicio que obtiene los lotes
        var lots = await _lotService.ObtenerLotesAsync();

        Lotes.Clear();
        foreach (var lot in lots)
            Lotes.Add(lot);
    }

    // CORRECCIÓN 2: Usar la lógica de éxito/error del servicio
    private async Task EliminarAsync(Lot lote)
    {
        if (lote == null) return;

        try
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Confirmar",
                $"¿Desea eliminar permanentemente el lote {lote.Code}?",
                "Eliminar",
                "Cancelar");

            if (!confirm) return;

            // LLAMADA AL SERVICIO (Igual que en Actividades)
            var (success, message) = await _lotService.EliminarLoteAsync(lote.Id);

            if (success)
            {
                await Shell.Current.DisplayAlertAsync("Éxito", message, "OK");
                await LoadAsync();
            }
            else
            {
                // Aquí mostrará el mensaje de "No se puede eliminar porque tiene registros asociados"
                await Shell.Current.DisplayAlertAsync("Información", message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Error inesperado: {ex.Message}", "OK");
            System.Diagnostics.Debug.WriteLine($"Error al eliminar lote: {ex.Message}");
        }
    }
}