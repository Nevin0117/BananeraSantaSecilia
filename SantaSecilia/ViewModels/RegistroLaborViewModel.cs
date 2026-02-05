using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantaSecilia.ViewModels
{
    public partial class RegistroLaborViewModel : ObservableObject
    {
        private static readonly Dictionary<string, double> ActividadesTarifas = new()
        {
            { "Salario mínimo convencional", 0.7011 },
            { "Hacer puente para conchero", 0.7790 },
            { "Regar herbicida", 0.7790 },
            { "Chequear nemátodos", 0.7790 },
            { "Desinfectar herramientas", 0.7790 },
            { "Fumigar bolsas", 0.7790 },
            { "Limpiar empacadora", 0.7790 },
            { "Botar pizote", 0.7790 },
            { "Banderero", 0.7790 },
            { "Nivelar caminos", 0.8012 },
            { "Ayudante mecánico", 0.8012 },
            { "Ayudante soldador", 0.8012 },
            { "Celador", 0.8123 },
            { "Sanidad (fin de vivienda/baños)", 0.8123 },
            { "Mecánico", 1.0126 },
            { "Soldador", 1.0126 },
            { "Carpintero", 1.0126 },
            { "Ayudante irrigación", 0.8266 },
            { "Cedaceros", 0.7053 },
            { "Reapuntalar bananal", 0.7011 },
            { "Sembrar leguminosas/gramíneas", 0.7715 },
            { "Control de Sigatoka (deshoje)", 0.9368 },
            { "Desviar banderilla", 0.7164 },
            { "Sacar matas hospederas", 0.7935 },
            { "Mantenimiento hierbas canales", 0.7715 },
            { "Aporcar semilleros", 0.7164 },
            { "Tiburón (herramienta)", 0.7584 },
            { "Mantenimiento de plantillo", 0.9042 },
            { "Mantenimiento de semillero", 0.8955 },
            { "Apuntalar con bambú", 0.7164 },
            { "Corregir población", 0.8955 },
            { "Desviar fruta orilla caminos", 0.7011 },
            { "Cargar bambú", 0.7011 },
            { "Limpieza de boquetes", 0.7011 }
        };

        public List<string> Actividades { get; } = ActividadesTarifas.Keys.OrderBy(x => x).ToList();

        [ObservableProperty]
        private string _trabajadorBusqueda = "";

        [ObservableProperty]
        private string _actividadSeleccionada = "";

        [ObservableProperty]
        private string _loteSeleccionado = "";

        [ObservableProperty]
        private double _horasTrabajadas = 0;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _errorMessage = "";

        private double TarifaActual
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ActividadSeleccionada) &&
                    ActividadesTarifas.TryGetValue(ActividadSeleccionada, out double tarifa))
                {
                    return tarifa;
                }
                return 0;
            }
        }

        public string MontoAutogenerado => (HorasTrabajadas * TarifaActual).ToString("F2");

        partial void OnHorasTrabajadasChanged(double value)
        {
            OnPropertyChanged(nameof(MontoAutogenerado));
        }

        partial void OnActividadSeleccionadaChanged(string value)
        {
            OnPropertyChanged(nameof(MontoAutogenerado));
        }

        [RelayCommand]
        private async Task GuardarRegistroAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(TrabajadorBusqueda))
                {
                    ErrorMessage = "Ingresa el nombre del trabajador";
                    return;
                }

                if (string.IsNullOrWhiteSpace(ActividadSeleccionada))
                {
                    ErrorMessage = "Selecciona una actividad";
                    return;
                }

                if (string.IsNullOrWhiteSpace(LoteSeleccionado))
                {
                    ErrorMessage = "Selecciona un lote";
                    return;
                }

                await Task.Delay(600);
                ErrorMessage = "Registro guardado correctamente";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private Task CancelarAsync()
        {
            ErrorMessage = string.Empty;
            return Task.CompletedTask;
        }
    }
}
