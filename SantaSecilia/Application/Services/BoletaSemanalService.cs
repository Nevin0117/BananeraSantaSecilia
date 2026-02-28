using Microsoft.EntityFrameworkCore;
using SantaSecilia.Application.DTOs;
using SantaSecilia.Infrastructure.Data;

namespace SantaSecilia.Application.Services
{
    public class BoletaSemanalService
    {
        private readonly AppDbContext _context;

        public BoletaSemanalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> ObtenerTrabajadoresAsync()
        {
            return await _context.Workers
                .Select(w => w.FullName)
                .ToListAsync();
        }

        public async Task<BoletaSemanalDto> GenerarBoletaAsync(string trabajador, DateTime fecha)
        {
            var inicio = fecha.AddDays(-(int)fecha.DayOfWeek);
            var fin = inicio.AddDays(6);

            var dto = new BoletaSemanalDto
            {
                Trabajador = trabajador,
                FechaInicio = inicio,
                FechaFin = fin
            };

            var worker = await _context.Workers
                .FirstOrDefaultAsync(w => w.FullName == trabajador);

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
                    Tarifa = (decimal) line.RateSnapshot
                }
            ).ToListAsync();

            dto.Actividades = filas;

            return dto;

        }
    }
}