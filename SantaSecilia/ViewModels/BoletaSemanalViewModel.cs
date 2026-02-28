using Microsoft.EntityFrameworkCore;
using SantaSecilia.Application.Services;
using SantaSecilia.Application.DTOs;
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
        private readonly BoletaSemanalService _boletaService;

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null!)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ObservableCollection<string> Trabajadores { get; set; } = new();

        string trabajadorSeleccionado = "";
        public string TrabajadorSeleccionado
        {
            get => trabajadorSeleccionado;
            set
            {
                trabajadorSeleccionado = value;
                OnPropertyChanged();
                _ = GenerarBoleta();
            }
        }

        DateTime fechaSeleccionada = DateTime.Today;
        public DateTime FechaSeleccionada
        {
            get => fechaSeleccionada;
            set
            {
                fechaSeleccionada = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RangoSemana));
                _ = GenerarBoleta();
            }
        }

        public string RangoSemana
        {
            get
            {
                var inicio = FechaSeleccionada.AddDays(-(int)FechaSeleccionada.DayOfWeek);
                var fin = inicio.AddDays(6);

                return $"{inicio:dd/MM/yyyy} - {fin:dd/MM/yyyy}";
            }
        }

        public ObservableCollection<BoletaFila> Filas { get; set; } = new();


        decimal totalDevengado;
        public decimal TotalDevengado{
            get => totalDevengado;
            set
            {
                totalDevengado = value;
                OnPropertyChanged();
            }
        }

        decimal descuentos;
        public decimal Descuentos{
            get => descuentos;
            set
            {
                descuentos = value;
                OnPropertyChanged();
            }
        }

        decimal totalPagar;
        public decimal TotalPagar{
            get => totalPagar;
            set
            {
                totalPagar = value;
                OnPropertyChanged();
            }
        }

        public BoletaSemanalViewModel(BoletaSemanalService boletaService)
        {
            _boletaService = boletaService;
            _ = CargarTrabajadores();
        }

        public async Task CargarDatos(){
            await CargarTrabajadores();
            await GenerarBoleta();
        }

        public async Task GenerarBoleta()
        {
            if (string.IsNullOrEmpty(TrabajadorSeleccionado))
                return;

            var dto = await _boletaService.GenerarBoletaAsync(
                TrabajadorSeleccionado,
                FechaSeleccionada
            );

            Filas.Clear();

            foreach (var act in dto.Actividades)
            {
                Filas.Add(new BoletaFila
                {
                    Fecha = act.Fecha,
                    Actividad = act.Actividad,
                    Horas = act.Horas,
                    Tarifa = act.Tarifa
                });
            }
            OnPropertyChanged(nameof(Filas));

            TotalDevengado = Filas.Sum(f => f.Monto);
            Descuentos = dto.SeguroSocial + dto.SeguroEducativo + dto.Sindicato;
            TotalPagar = TotalDevengado - Descuentos;

            OnPropertyChanged(nameof(TotalDevengado));
            OnPropertyChanged(nameof(Descuentos));
            OnPropertyChanged(nameof(TotalPagar));
        }

        async Task CargarTrabajadores(){
            var workers = await _boletaService.ObtenerTrabajadoresAsync();

            Trabajadores.Clear();

            foreach (var w in workers)
                Trabajadores.Add(w);
        }
    }
}