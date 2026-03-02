using SantaSecilia.Application.Services;
using SantaSecilia.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace SantaSecilia.ViewModels
{
    public class TrabajadorFormViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly WorkerService _workerService;

        // Campos privados para manejar el estado y la limpieza
        private string _nombre = "";
        private string _cedula = "";
        private string _estadoSeleccionado = "Activo";

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        public string Cedula
        {
            get => _cedula;
            set
            {
                // Filtro: Solo permite Letras, Números y Guiones. Remueve todo lo demás.
                _cedula = Regex.Replace(value ?? "", @"[^a-zA-Z0-9-]", "").ToUpper();
                OnPropertyChanged(nameof(Cedula));
            }
        }

        public string EstadoSeleccionado
        {
            get => _estadoSeleccionado;
            set { _estadoSeleccionado = value; OnPropertyChanged(nameof(EstadoSeleccionado)); }
        }

        public ObservableCollection<string> Estados { get; } = new ObservableCollection<string> { "Activo", "Inactivo" };
        public ICommand GuardarCommand { get; }

        public TrabajadorFormViewModel(WorkerService workerService)
        {
            _workerService = workerService;
            GuardarCommand = new Command(async () => await Guardar());
        }

        async Task Guardar()
        {
            // Validaciones de campos vacíos
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                await Shell.Current.DisplayAlertAsync("Campo Requerido", "El nombre es obligatorio", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Cedula))
            {
                await Shell.Current.DisplayAlertAsync("Campo Requerido", "La cédula es obligatoria", "OK");
                return;
            }

            // Validación de Cédula Duplicada (Para evitar el crash)
            bool existe = await _workerService.ExisteTrabajadorConCedulaAsync(Cedula);
            if (existe)
            {
                await Shell.Current.DisplayAlertAsync("Cédula Duplicada",
                    $"Ya existe un trabajador registrado con la cédula: {Cedula}", "OK");
                return;
            }

            try
            {
                var worker = new Worker
                {
                    FullName = Nombre.Trim(),
                    IdentificationNumber = Cedula.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = EstadoSeleccionado == "Activo"
                };

                await _workerService.AgregarTrabajadorAsync(worker);

                await Shell.Current.DisplayAlertAsync("Éxito", "Trabajador guardado correctamente", "OK");

                // Limpiar formulario para el próximo uso
                LimpiarFormulario();

                // Regresar a la lista
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"No se pudo guardar: {ex.Message}", "OK");
            }
        }

        public void LimpiarFormulario()
        {
            Nombre = string.Empty;
            Cedula = string.Empty;
            EstadoSeleccionado = "Activo";
        }
    }
}