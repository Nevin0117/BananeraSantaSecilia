using Microsoft.EntityFrameworkCore;
using SantaSecilia.Application.DTOs;
using SantaSecilia.Infrastructure.Data;
using SantaSecilia.Domain.Entities; // <-- NUEVO: Necesario para que reconozca a 'Worker'

namespace SantaSecilia.Application.Services
{
    public class BoletaSemanalService
    {
        private readonly AppDbContext _context;

        public BoletaSemanalService(AppDbContext context)
        {
            _context = context;
        }

        // --- CAMBIO AQUÍ: Ahora devuelve Task<List<Worker>> ---
        public async Task<List<Worker>> ObtenerTrabajadoresAsync()
        {
            // Quitamos el .Select() para que devuelva el objeto completo
            return await _context.Workers
                .OrderBy(w => w.FullName) // Sugerencia pro: los ordenamos alfabéticamente
                .ToListAsync();
        }

        public async Task<BoletaSemanalDto> GenerarBoletaAsync(string trabajador, DateTime fecha)
        {
            var inicio = fecha.AddDays(-(int)fecha.DayOfWeek).Date;
            var fin = inicio.AddDays(6);

            var worker = await _context.Workers
                            .FirstOrDefaultAsync(w => w.FullName == trabajador);

            var dto = new BoletaSemanalDto
            {
                Trabajador = trabajador,
                FechaInicio = inicio,
                FechaFin = fin,
                // ASIGNACIÓN DE DATOS PARA EL CUADRO LATERAL
                CodigoTrabajador = worker?.Id.ToString() ?? "---",
                CedulaTrabajador = worker?.IdentificationNumber ?? "---"
            };


            if (worker == null)
                return dto;

            var filas = await (
                from dr in _context.DailyRecords
                join line in _context.DailyRecordLines on dr.Id equals line.DailyRecordId
                join act in _context.Activities on line.ActivityId equals act.Id
                where dr.WorkerId == worker.Id
                   && dr.WorkDate >= inicio
                   && dr.WorkDate <= fin

                select new BoletaActividadDto
                {
                    Fecha = dr.WorkDate,
                    Actividad = act.Name,
                    Horas = line.Hours,
                    Tarifa = (decimal)line.RateSnapshot
                }
            ).ToListAsync();

            dto.Actividades = filas;

            dto.TotalDevengado = filas.Sum(f => f.Monto);

            dto.SeguroSocial = Math.Round(dto.TotalDevengado * 0.0975m, 2);
            dto.SeguroEducativo = Math.Round(dto.TotalDevengado * 0.0125m, 2);
            dto.Sindicato = 1.00m;

            dto.Descuentos = dto.SeguroSocial + dto.SeguroEducativo + dto.Sindicato;
            dto.TotalPagar = dto.TotalDevengado - dto.Descuentos;

            return dto;
        }
    }
}