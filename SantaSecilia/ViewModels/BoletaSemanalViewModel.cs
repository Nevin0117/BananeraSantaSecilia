using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
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
        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = null!)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ObservableCollection<string> Trabajadores { get; set; } = new();
        public ObservableCollection<string> Semanas { get; set; } = new();

        string trabajadorSeleccionado;
        public string TrabajadorSeleccionado
        {
            get => trabajadorSeleccionado;
            set
            {
                trabajadorSeleccionado = value;
                OnPropertyChanged();
            }
        }

        string semanaSeleccionada;
        public string SemanaSeleccionada
        {
            get => semanaSeleccionada;
            set
            {
                semanaSeleccionada = value;
                OnPropertyChanged();
            }
        }

        // ==============================
        // Tabla
        // ==============================

        public ObservableCollection<BoletaFila> Filas { get; set; } = new();


        // ==============================
        // Totales
        // ==============================

        public decimal TotalDevengado => Filas.Sum(f => f.Monto);

        // TODO: obtener desde WeeklyPayStub.SsDeduction (tasa seguro social aplicada al devengado bruto)
        public decimal Descuentos => 0;

        // TODO: obtener desde WeeklyPayStub.SeDeduction (tasa seguro educativo aplicada al devengado bruto)

        // TODO: obtener desde WeeklyPayStub.UnionDues (cuota fija semanal del sindicato)

        public decimal TotalPagar => TotalDevengado - Descuentos;



#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        public BoletaSemanalViewModel() {
            //Info para la BD
        }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    }
}