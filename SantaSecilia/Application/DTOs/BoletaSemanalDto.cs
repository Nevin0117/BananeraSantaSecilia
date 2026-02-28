namespace SantaSecilia.Application.DTOs;

public class BoletaActividadDto{
    public DateTime Fecha { get; set; }
    public string Actividad { get; set; } = "";
    public int Horas { get; set; }
    public decimal Tarifa { get; set; }
    public decimal Monto => Horas * Tarifa;
}

public class BoletaSemanalDto{
    public string Trabajador { get; set; } = "";

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    public string SemanaTexto => $"{FechaInicio:dd/MM/yy} - {FechaFin:dd/MM/yy}";

    public List<BoletaActividadDto> Actividades { get; set; } = new();

    public decimal TotalDevengado { get; set; }
    public decimal SeguroSocial { get; set; }
    public decimal SeguroEducativo { get; set; }
    public decimal Sindicato { get; set; }
    public decimal Descuentos { get; set; }
    public decimal TotalPagar { get; set; }
}