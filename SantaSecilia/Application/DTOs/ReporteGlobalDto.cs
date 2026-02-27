namespace SantaSecilia.Application.DTOs;

public class ActividadReporteDto
{
    public string NombreActividad { get; set; } = "";
    public decimal HorasTotales { get; set; }
    public decimal TarifaPorHora { get; set; }
    public decimal TotalPorActividad { get; set; }
}

public class ReporteGlobalDto
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string SemanaTexto => $"{FechaInicio:dd/MM/yy} - {FechaFin:dd/MM/yy}";
    public List<ActividadReporteDto> Actividades { get; set; } = new();
    public decimal TotalPagado { get; set; }


    // Propiedades para interfaz
    public int TotalActividades => Actividades.Count;
    public decimal TotalHoras => Actividades.Sum(a => a.HorasTotales);
    public int TotalJornaleros { get; set; }
}