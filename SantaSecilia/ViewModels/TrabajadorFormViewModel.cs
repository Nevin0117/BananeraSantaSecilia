using SantaSecilia.Application.Services;
using SantaSecilia.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SantaSecilia.ViewModels
{
    public class TrabajadorFormViewModel: INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly WorkerService _workerService;
        public string Nombre { get; set; } = "";
        public string Cedula { get; set; } = "";
        public ObservableCollection<string> Estados { get; } = new ObservableCollection<string> { "Activo", "Inactivo" };

        public ICommand GuardarCommand { get; }

        [Obsolete]
        public TrabajadorFormViewModel(WorkerService workerService)
        {
            _workerService = workerService;
            GuardarCommand = new Command(async () => await Guardar());
        }

        string estadoSeleccionado = "Activo";
        public string EstadoSeleccionado
        {
            get => estadoSeleccionado;
            set
            {
                estadoSeleccionado = value;
                OnPropertyChanged(nameof(EstadoSeleccionado));
            }
        }

        void Limpiar(){
            Nombre = "";
            Cedula = "";
        }

        string name = "";
        public string nombre
        {
            get => nombre;
            set
            {
                nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        string cedul = "";
        public string cedula
        {
            get => cedula;
            set
            {
                cedula = value;
                OnPropertyChanged(nameof(Cedula));
            }
        }




        [Obsolete]
        async Task Guardar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                await Shell.Current.DisplayAlert("Error", "El nombre es obligatorio", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Cedula))
            {
                await Shell.Current.DisplayAlert("Error", "La cédula es obligatoria", "OK");
                return;
            }

            var worker = new Worker
            {
                FullName = Nombre,
                IdentificationNumber = Cedula,
                CreatedAt = DateTime.UtcNow,
                IsActive = EstadoSeleccionado == "Activo"
            };

            await _workerService.AgregarTrabajadorAsync(worker);

            Limpiar();

            await Shell.Current.GoToAsync("..");
        }
    }
}