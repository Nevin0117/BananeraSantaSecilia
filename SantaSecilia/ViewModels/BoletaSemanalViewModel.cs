using Microsoft.EntityFrameworkCore;
using SantaSecilia.Application.Services;
using SantaSecilia.Application.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.IO;

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
        private bool _hayDatos = false;
        

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null!)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ObservableCollection<string> TrabajadoresSugeridos { get; set; } = new();

        private string _trabajadorBusqueda = "";
        public string TrabajadorBusqueda
        {
            get => _trabajadorBusqueda;
            set
            {
                _trabajadorBusqueda = value;
                OnPropertyChanged();
                // Disparamos la búsqueda cuando el usuario escribe
                _ = FiltrarTrabajadoresAsync(value);
            }
        }

        private bool _mostrarSugerencias;
        public bool MostrarSugerencias
        {
            get => _mostrarSugerencias;
            set { _mostrarSugerencias = value; OnPropertyChanged(); }
        }

        // El comando que se ejecuta al tocar un nombre en la lista
        public ICommand SelectWorkerCommand { get; }

        // --------------------------------------------
        public ICommand ImprimirCommand { get; }

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



        // 1. Iniciamos en null (o una fecha nula) para que se vea el placeholder
        private DateTime? fechaSeleccionada = null;

        public DateTime? FechaSeleccionada
        {
            get => fechaSeleccionada;
            set
            {
                if (fechaSeleccionada != value)
                {
                    fechaSeleccionada = value;
                    OnPropertyChanged(nameof(FechaSeleccionada)); // Esto disparará el evento en el code-behind
                    OnPropertyChanged(nameof(RangoSemana));

                    if (value.HasValue)
                    {
                        _ = GenerarBoleta();
                    }
                }
            }
        }

    // 3. Esta propiedad la usaremos en el XAML para IsVisible y para el Color
    public bool FechaFueSeleccionada => FechaSeleccionada.HasValue;

        public string RangoSemana
        {
            get
            {
                // Si no hay fecha, devolvemos un texto vacío o guiones
                if (!FechaSeleccionada.HasValue)
                    return "---";

                // Usamos .Value para acceder al DateTime real dentro del nullable
                DateTime fecha = FechaSeleccionada.Value;

                // Ajuste para que la semana empiece en DOMINGO
                // Calculamos cuántos días han pasado desde el último domingo
                int diff = (int)fecha.DayOfWeek;

                // Restamos esa diferencia para llegar al domingo anterior
                DateTime inicio = fecha.AddDays(-1 * diff).Date;

                // Sumamos 6 días para llegar al sábado
                DateTime fin = inicio.AddDays(6).Date;

                // Retornamos el rango formateado (Ejemplo: 01 Mar - 07 Mar, 2026)
                return $"{inicio:dd MMM} - {fin:dd MMM, yyyy}".ToUpper(); // En mayúsculas para seguir el estilo
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

        // Propiedades para datos de trabajador
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
            SelectWorkerCommand = new Command<string>(OnSelectWorker);
            _ = CargarTrabajadores();
        }

        public async Task<(byte[] PdfBytes, string FileName)> PrepararBoletaPDFAsync()
        {
            if (!HayDatos || string.IsNullOrEmpty(TrabajadorSeleccionado))
                throw new Exception("No hay datos cargados para este colaborador.");

            // 1. Preparar los registros para el generador
            var registrosPDF = Filas.Select(f => new BoletaSemanalPDFGenerator.RegistroActividad
            {
                Fecha = f.Fecha.ToString("dd/MM/yy"),
                Actividad = f.Actividad,
                Horas = f.Horas.ToString(),
                Tarifa = f.Tarifa.ToString("F2"),
                Monto = $"B/. {f.Monto:F2}"
            }).ToList();

            // 2. Cargar el logo
            byte[] logoBytes;
            using (var stream = await FileSystem.OpenAppPackageFileAsync("logo.png"))
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                logoBytes = ms.ToArray();
            }

            // 3. Generar el PDF
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

            // Nombre sugerido inicial
            string nombreArchivo = $"Boleta_{TrabajadorSeleccionado.Replace(" ", "_")}_{DateTime.Now:ddMMyy}.pdf";

            return (pdfBytes, $"Boleta_{TrabajadorSeleccionado.Replace(" ", "_")}.pdf");
        }

        private async Task FiltrarTrabajadoresAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                TrabajadoresSugeridos.Clear();
                MostrarSugerencias = false;
                // Opcional: Limpiar la boleta si borran el buscador
                // TrabajadorSeleccionado = ""; 
                return;
            }

            if (query.Length < 2)
            {
                TrabajadoresSugeridos.Clear();
                MostrarSugerencias = false;
                return;
            }

            var filtrados = Trabajadores
                .Where(w => w.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Take(10) // Limitamos a 10 para no saturar la vista
                .ToList();

            TrabajadoresSugeridos.Clear();
            foreach (var w in filtrados)
                TrabajadoresSugeridos.Add(w);

            MostrarSugerencias = TrabajadoresSugeridos.Any();
        }

        private void OnSelectWorker(string nombre)
        {
            TrabajadorSeleccionado = nombre; // Esto dispara GenerarBoleta()
            _trabajadorBusqueda = nombre;    // Actualiza el texto del Entry sin disparar filtro
            OnPropertyChanged(nameof(TrabajadorBusqueda));

            TrabajadoresSugeridos.Clear();
            MostrarSugerencias = false;
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
            // 1. Primero cargamos la lista de nombres para el buscador
            await CargarTrabajadores();

            // 2. Solo intentamos generar la boleta si ya hay una fecha y un trabajador seleccionados
            // (Al abrir la pantalla por primera vez, estos suelen ser null o vacíos, 
            // por lo que no queremos que falle intentando buscar datos inexistentes)
            if (FechaSeleccionada.HasValue && !string.IsNullOrEmpty(TrabajadorSeleccionado))
            {
                await GenerarBoleta();
            }
        }

        public async Task GenerarBoleta()
        {
            if (!FechaSeleccionada.HasValue) return;

            DateTime fechaReal = FechaSeleccionada.Value;

            // Si no hay trabajador, no hay datos que mostrar
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

                if (dto.Actividades != null && dto.Actividades.Any())
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
                    // AQUÍ ASIGNAMOS LOS VALORES AL CUADRO LATERAL
                    // Asumiendo que tu DTO tiene estas propiedades (ajusta los nombres si varían)
                    CodigoTrabajador = dto.CodigoTrabajador ?? "N/A";
                    CedulaTrabajador = dto.CedulaTrabajador ?? "N/A";

                    HayDatos = dto.Actividades.Any();
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



        async Task CargarTrabajadores(){
            var workers = await _boletaService.ObtenerTrabajadoresAsync();

            Trabajadores.Clear();

            foreach (var w in workers)
                Trabajadores.Add(w);
        }
    }
}