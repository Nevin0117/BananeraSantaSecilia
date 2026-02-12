using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using SantaSecilia.Views;
using System.Windows.Input;

namespace SantaSecilia.ViewModels
{
    public class ActividadItem
    {
        public required string Actividad { get; set; }
        public required decimal Tarifa { get; set; }
        public string Estado => Activo ? "Activo" : "Inactivo";
        public bool Activo { get; set; }
    }

    public class ActividadesViewModel
    {
        public ObservableCollection<ActividadItem> Actividades { get; set; }
        public ICommand RegistrarCommand { get; }
        public ICommand EditarCommand { get; }

        public ActividadesViewModel()
        {
            Actividades = new ObservableCollection<ActividadItem>
            {
                new() { Actividad = "Banderero",          Tarifa = 0.7790m, Activo = true  },
                new() { Actividad = "Limpiar Empacadora", Tarifa = 0.7790m, Activo = true  },
                new() { Actividad = "Soldador",           Tarifa = 1.0126m, Activo = false }
            };

            RegistrarCommand = new Command(async () =>
                await Shell.Current.GoToAsync(nameof(RegistrarActividadPage)));

            EditarCommand = new Command<ActividadItem>(async (actividad) =>
                await Shell.Current.GoToAsync(nameof(EditarActividadPage)));
        }
    }
}