using Microsoft.EntityFrameworkCore;
using SantaSecilia.Application.Services;
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
        public bool Activo { get; set; }
        public string Estado => Activo ? "Activo" : "Inactivo";
    }

    public class TrabajadoresListViewModel : INotifyPropertyChanged
    {
        private readonly WorkerService _workerService;
        public ObservableCollection<TrabajadorItem> Trabajadores { get; set; } = new();
        public ICommand RegistrarCommand { get; }
        public ICommand EditarCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        void OnPropertyChanged([CallerMemberName] string name = null!){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public TrabajadoresListViewModel(WorkerService workerService)
        {
            _workerService = workerService;

            RegistrarCommand = new Command(async () =>
                await Shell.Current.GoToAsync(nameof(TrabajadorFormPage)));

            EditarCommand = new Command<TrabajadorItem>(async (trabajador) =>
            {
                await Shell.Current.GoToAsync(
                    $"{nameof(EditarTrabajadorPage)}?TrabajadorId={trabajador.Codigo}");
            });
        }

        public async Task CargarTrabajadoresAsync()
        {
            var workers = await _workerService.ObtenerTrabajadoresAsync();

            Trabajadores.Clear();

            foreach (var w in workers)
            {
                Trabajadores.Add(new TrabajadorItem
                {
                    Codigo = w.Id,
                    Nombre = w.FullName,
                    Cedula = w.IdentificationNumber,
                    Activo = w.IsActive
                });
            }
            Filtrar();
        }

        public async void Recargar()
        {
            await CargarTrabajadoresAsync();
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