using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SantaSecilia.Application.DTOs;
using SantaSecilia.Application.Services;
using SantaSecilia.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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

    // CORRECCIÓN 1: Agregamos "partial" a la clase
    public partial class BoletaSemanalViewModel : INotifyPropertyChanged
    {
        private readonly BoletaSemanalService _boletaService;
        private bool _hayDatos = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null!)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // CORRECCIÓN 2: Dejamos solo UNA declaración de las colecciones como Worker
        public ObservableCollection<Worker> TrabajadoresSugeridos { get; set; } = new();
        public ObservableCollection<Worker> Trabajadores { get; set; } = new();

        private string _trabajadorBusqueda = "";
        public string TrabajadorBusqueda
        {
            get => _trabajadorBusqueda;
            set
            {
                _trabajadorBusqueda = value;
                OnPropertyChanged();
                _ = FiltrarTrabajadoresAsync(value);
            }
        }

        private bool _mostrarSugerencias;
        public bool MostrarSugerencias
        {
            get => _mostrarSugerencias;
            set { _mostrarSugerencias = value; OnPropertyChanged(); }
        }

        public ICommand ImprimirCommand { get; }

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

        private DateTime? fechaSeleccionada = null;
        public DateTime? FechaSeleccionada
        {
            get => fechaSeleccionada;
            set
            {
                if (fechaSeleccionada != value)
                {
                    fechaSeleccionada = value;
                    OnPropertyChanged(nameof(FechaSeleccionada));
                    OnPropertyChanged(nameof(RangoSemana));

                    if (value.HasValue)
                    {
                        _ = GenerarBoleta();
                    }
                }
            }
        }

        public bool FechaFueSeleccionada => FechaSeleccionada.HasValue;

        public string RangoSemana
        {
            get
            {
                if (!FechaSeleccionada.HasValue)
                    return "---";

                DateTime fecha = FechaSeleccionada.Value;
                int diff = (int)fecha.DayOfWeek;
                DateTime inicio = fecha.AddDays(-1 * diff).Date;
                DateTime fin = inicio.AddDays(6).Date;

                return $"{inicio:dd MMM} - {fin:dd MMM, yyyy}".ToUpper();
            }
        }

        public ObservableCollection<BoletaFila> Filas { get; set; } = new();

        decimal totalDevengado;
        public decimal TotalDevengado
        {
            get => totalDevengado;
            set { totalDevengado = value; OnPropertyChanged(); }
        }

        decimal descuentos;
        public decimal Descuentos
        {
            get => descuentos;
            set { descuentos = value; OnPropertyChanged(); }
        }

        decimal totalPagar;
        public decimal TotalPagar
        {
            get => totalPagar;
            set { totalPagar = value; OnPropertyChanged(); }
        }

        private string _codigoTrabajador = "---";
        public string CodigoTrabajador
        {
            get => _codigoTrabajador;
            set { _codigoTrabajador = value; OnPropertyChanged(); }
        }

        private string _cedulaTrabajador = "---";
        public string CedulaTrabajador
        {
            get => _cedulaTrabajador;
            set { _cedulaTrabajador = value; OnPropertyChanged(); }
        }

        public BoletaSemanalViewModel(BoletaSemanalService boletaService)
        {
            _boletaService = boletaService;
            ImprimirCommand = new Command(async () => await ImprimirAsync());
            _ = CargarTrabajadores();
        }

        private async Task ImprimirAsync()
        {
            
            await Task.CompletedTask;
        }

        public async Task<(byte[] PdfBytes, string FileName)> PrepararBoletaPDFAsync()
        {
            if (!HayDatos || string.IsNullOrEmpty(TrabajadorSeleccionado))
                throw new Exception("No hay datos cargados para este colaborador.");

            var registrosPDF = Filas.Select(f => new BoletaSemanalPDFGenerator.RegistroActividad
            {
                Fecha = f.Fecha.ToString("dd/MM/yy"),
                Actividad = f.Actividad,
                Horas = f.Horas.ToString(),
                Tarifa = f.Tarifa.ToString("F2"),
                Monto = $"B/. {f.Monto:F2}"
            }).ToList();

            byte[] logoBytes;
            using (var stream = await FileSystem.OpenAppPackageFileAsync("logo.png"))
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                logoBytes = ms.ToArray();
            }

            var pdfBytes = BoletaSemanalPDFGenerator.GenerarPDF(
                CodigoTrabajador,
                CedulaTrabajador,
                TrabajadorSeleccionado,
                RangoSemana,
                registrosPDF,
                $"B/. {TotalDevengado:F2}",
                $"B/. {Descuentos:F2}",
                $"B/. {TotalPagar:F2}",
                logoBytes
            );

            return (pdfBytes, $"Boleta_{TrabajadorSeleccionado.Replace(" ", "_")}.pdf");
        }

        private async Task FiltrarTrabajadoresAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                TrabajadoresSugeridos.Clear();
                MostrarSugerencias = false;
                return;
            }

            var filtrados = Trabajadores
                .Where(w => w.FullName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            w.IdentificationNumber.Contains(query))
                .Take(10)
                .ToList();

            TrabajadoresSugeridos.Clear();
            foreach (var w in filtrados)
                TrabajadoresSugeridos.Add(w);

            MostrarSugerencias = TrabajadoresSugeridos.Any();
        }

        // CORRECCIÓN 4: Al poner [RelayCommand] aquí, se genera un 'SelectWorkerCommand' automático.
        // Por eso borramos las otras declaraciones repetidas arriba.
        [RelayCommand]
        private void SelectWorker(Worker worker)
        {
            if (worker == null) return;

            CodigoTrabajador = worker.Id.ToString();
            TrabajadorSeleccionado = worker.FullName;
            CedulaTrabajador = worker.IdentificationNumber;
            

            _trabajadorBusqueda = worker.DisplayInfo;
            OnPropertyChanged(nameof(TrabajadorBusqueda));

            TrabajadoresSugeridos.Clear();
            MostrarSugerencias = false;

            _ = GenerarBoleta();
        }

        public bool HayDatos
        {
            get => _hayDatos;
            set
            {
                if (_hayDatos != value)
                {
                    _hayDatos = value;
                    OnPropertyChanged(nameof(HayDatos));
                    OnPropertyChanged(nameof(MostrarMensajeVacio));
                }
            }
        }

        public bool MostrarMensajeVacio => !HayDatos;

        public async Task CargarDatos()
        {
            await CargarTrabajadores();

            if (FechaSeleccionada.HasValue && !string.IsNullOrEmpty(TrabajadorSeleccionado))
            {
                await GenerarBoleta();
            }
        }

        public async Task GenerarBoleta()
        {
            if (!FechaSeleccionada.HasValue) return;

            if (string.IsNullOrEmpty(TrabajadorSeleccionado))
            {
                HayDatos = false;
                return;
            }

            try
            {
                var dto = await _boletaService.GenerarBoletaAsync(
                    TrabajadorSeleccionado,
                    FechaSeleccionada.Value
                );

                Filas.Clear();

                if (dto.Actividades != null && dto.Actividades.Count > 0)
                {
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

                    TotalDevengado = Filas.Sum(f => f.Monto);
                    Descuentos = dto.SeguroSocial + dto.SeguroEducativo + dto.Sindicato;
                    TotalPagar = TotalDevengado - Descuentos;

                    HayDatos = true;
                }
                else
                {
                    HayDatos = false;
                    TotalDevengado = 0;
                    Descuentos = 0;
                    TotalPagar = 0;
                }

                if (dto != null)
                {
                    CodigoTrabajador = dto.CodigoTrabajador ?? "N/A";
                    CedulaTrabajador = dto.CedulaTrabajador ?? "N/A";

                    HayDatos = dto.Actividades?.Count > 0;
                }
            }
            catch (Exception ex)
            {
                CodigoTrabajador = "---";
                CedulaTrabajador = "---";
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                HayDatos = false;
            }
        }

        async Task CargarTrabajadores()
        {
            var workers = await _boletaService.ObtenerTrabajadoresAsync();
            Trabajadores.Clear();
            foreach (var w in workers)
                Trabajadores.Add(w);
        }
    }
}