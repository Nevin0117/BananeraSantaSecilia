using Microsoft.EntityFrameworkCore;
using SantaSecilia.Infrastructure.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SantaSecilia.ViewModels
{
    public class BoletaFila
    {
        public DateTime Fecha { get; set; }
        public string Actividad { get; set; } = "";
        public int Horas { get; set; }
        public decimal Tarifa { get; set; }
        public decimal Monto => Horas * Tarifa;
    }


    public class BoletaSemanalViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext contex;

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = null!)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ObservableCollection<string> Trabajadores { get; set; } = new();
        public ObservableCollection<string> Semanas { get; set; } = new();

        string trabajadorSeleccionado = "";
        public string TrabajadorSeleccionado
        {
            get => trabajadorSeleccionado;
            set
            {
                trabajadorSeleccionado = value;
                OnPropertyChanged();
            }
        }

        string semanaSeleccionada = "";
        public string SemanaSeleccionada
        {
            get => semanaSeleccionada;
            set
            {
                semanaSeleccionada = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BoletaFila> Filas { get; set; } = new();

        public decimal TotalDevengado => Filas.Sum(f => f.Monto);

        public decimal Descuentos => 0;    

        public decimal TotalPagar => TotalDevengado - Descuentos;

        public BoletaSemanalViewModel(AppDbContext context)
        {
            contex = context;

            _ = CargarTrabajadores();
        }

        async Task CargarTrabajadores()
        {
            var workers = await contex.Workers.ToListAsync();

            Trabajadores.Clear();

            foreach (var w in workers)
                Trabajadores.Add(w.FullName);
                Trabajadores.Add("Prueba 1");
                Trabajadores.Add("Prueba 2");
        }
    }
}