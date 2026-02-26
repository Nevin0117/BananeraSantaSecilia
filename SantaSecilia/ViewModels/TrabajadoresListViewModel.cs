using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Data;
using SantaSecilia.Views;
using SQLitePCL;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.Windows.Input;

namespace SantaSecilia.ViewModels
{
    public class TrabajadorItem
    {
        public int Codigo { get; set; }
        public required string Nombre { get; set; }
        public required string Cedula { get; set; }
        public string Estado => Activo ? "Activo" : "Inactivo";
        public bool Activo { get; set; }
    }

    public class TrabajadoresListViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext context;
        public ObservableCollection<TrabajadorItem> Trabajadores { get; set; } = new();
        public ICommand RegistrarCommand { get; }
        public ICommand EditarCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        void OnPropertyChanged([CallerMemberName] string name = null!){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public TrabajadoresListViewModel(AppDbContext context)
        {
            this.context = context;

            RegistrarCommand = new Command(async () =>
                await Shell.Current.GoToAsync(nameof(TrabajadorFormPage)));

            EditarCommand = new Command<TrabajadorItem>(async (trabajador) =>
            {
                var parametros = new Dictionary<string, object>{
                    { "Trabajador", trabajador }
                };

                await Shell.Current.GoToAsync(nameof(EditarTrabajadorPage), parametros);
            });


            _ = CargarTrabajadores();
        }

        public async void Recargar()
        {
            await CargarTrabajadores();
        }

        async Task CargarTrabajadores()
        {
            _workersOriginal = await context.Workers.ToListAsync();
            Filtrar();
            
        }

        List<Worker> _workersOriginal = new();

        string _busqueda = "";
        public string Busqueda
        {
            get => _busqueda;
            set
            {
                _busqueda = value;
                OnPropertyChanged();
                Filtrar();
            }
        }

        void Filtrar()
        {
            Trabajadores.Clear();

            var filtrados = _workersOriginal
                .Where(w => string.IsNullOrWhiteSpace(_busqueda)
                || w.FullName.Contains(_busqueda, StringComparison.OrdinalIgnoreCase)
                || w.IdentificationNumber.Contains(_busqueda));

            foreach (var w in filtrados)
            {
                Trabajadores.Add(new TrabajadorItem
                {
                    Codigo = w.Id,
                    Nombre = w.FullName,
                    Cedula = w.IdentificationNumber,
                    Activo = w.IsActive
                });
            }
        }
    }
}