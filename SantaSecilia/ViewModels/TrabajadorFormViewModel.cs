using SantaSecilia.Application.Services;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SantaSecilia.ViewModels
{
    public class TrabajadorFormViewModel
    {
        private readonly WorkerService _workerService;
        public string Nombre { get; set; } = "";
        public string Cedula { get; set; } = "";
        public ObservableCollection<string> Estados { get; } =
            new ObservableCollection<string> { "Activo", "Inactivo" };

        public ICommand GuardarCommand { get; }

        [Obsolete]
        public TrabajadorFormViewModel(WorkerService workerService)
        {
            _workerService = workerService;
            GuardarCommand = new Command(async () => await Guardar());
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
                IsActive = true
            };

            await _workerService.AgregarTrabajadorAsync(worker);

            await Shell.Current.GoToAsync("..");
        }
    }
}