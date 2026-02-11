using System.Collections.ObjectModel;
using System.Windows.Input;
using SantaSecilia.Views;

namespace SantaSecilia.ViewModels
{
    public class LotesViewModel
    {
        public ObservableCollection<LoteItem> Lotes { get; set; }

        public ICommand RegistrarCommand { get; }
        public ICommand EditarCommand { get; }

        public LotesViewModel()
        {
            Lotes = new ObservableCollection<LoteItem>
            {
                new() { Codigo = 101, Activo = true },
                new() { Codigo = 102, Activo = false },
                new() { Codigo = 103, Activo = true },
                new() { Codigo = 104, Activo = false },
            };

            RegistrarCommand = new Command(async () =>
                await Shell.Current.GoToAsync(nameof(RegistrarLotesPage)));

            EditarCommand = new Command<LoteItem>(async (lote) =>
                await Shell.Current.GoToAsync(nameof(EditarLotesPage)));
        }
    }

    public class LoteItem
    {
        public int Codigo { get; set; }
        public bool Activo { get; set; }
        public string Estado => Activo ? "Activo" : "Inactivo";
    }
}


