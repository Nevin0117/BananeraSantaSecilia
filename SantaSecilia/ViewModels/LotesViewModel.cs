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

    public LotesViewModel(LotRepository repository)
    {
        _repository = repository;

        RegistrarCommand = new Command(async () =>
            await Shell.Current.GoToAsync(nameof(RegistrarLotesPage)));


        EditarCommand = new Command<Lot>(async (lot) =>
        {
            if (lot == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(EditarLotesPage)}?id={lot.Id}");
        });

    }

    public async Task LoadAsync()
    {
        var lots = await _repository.GetAllAsync();

        Lotes.Clear();

        foreach (var lot in lots)
            Lotes.Add(lot);
    }
}
