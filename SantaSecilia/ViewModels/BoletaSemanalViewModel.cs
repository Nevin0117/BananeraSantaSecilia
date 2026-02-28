using Microsoft.EntityFrameworkCore;
using SantaSecilia.Domain.Entities;
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

        public decimal SeguroSocial => TotalDevengado * 0.0975m;
        public decimal SeguroEducativo => TotalDevengado * 0.0125m;
        public decimal Sindicato => 2.50m;
        public decimal Descuentos => SeguroSocial + SeguroEducativo + Sindicato;

        public decimal TotalPagar => TotalDevengado - Descuentos;

        public BoletaSemanalViewModel(AppDbContext context)
        {
            contex = context;

            _ = CargarTrabajadores();
        }

        public async Task CargarDatos()
        {
            await CargarTrabajadores();
            CargarSemanas();
        }

        async Task CargarTrabajadores()
        {
            var workers = await contex.Workers.ToListAsync();

            Trabajadores.Clear();

            foreach (var w in workers)
                Trabajadores.Add(w.FullName);
        }

        void CargarSemanas()
        {
            Semanas.Clear();

            var hoy = DateTime.Today;

            for (int i = 0; i < 6; i++)
            {
                var inicio = hoy.AddDays(-7 * i);
                var fin = inicio.AddDays(6);

                Semanas.Add($"{inicio:dd/MM/yyyy} - {fin:dd/MM/yyyy}");
            }
        }

        public void RefrescarTotales()
        {
            OnPropertyChanged(nameof(TotalDevengado));
            OnPropertyChanged(nameof(TotalPagar));
        }

    }
}