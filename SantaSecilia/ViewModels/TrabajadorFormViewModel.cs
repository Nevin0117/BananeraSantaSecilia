using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SantaSecilia.ViewModels
{
    public class TrabajadorFormViewModel
    {
        private readonly AppDbContext _context;
        public string Nombre { get; set; } = "";
        public string Cedula { get; set; } = "";
        public ObservableCollection<string> Estados { get; } =
            new ObservableCollection<string> { "Activo", "Inactivo" };

        public ICommand GuardarCommand { get; }

        public TrabajadorFormViewModel(AppDbContext context)
        {
            _context = context;
            GuardarCommand = new Command(async () => await Guardar());
        }

        async Task Guardar()
        {
            var worker = new Worker
            {
                FullName = Nombre,
                IdentificationNumber = Cedula,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            await Shell.Current.GoToAsync("..");
        }
    }
}