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
            return dto;

        }
    }
}