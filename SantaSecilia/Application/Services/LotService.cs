using System;
using System.Collections.Generic;
using System.Text;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Repositories;

namespace SantaSecilia.Application.Services
{
    public class LotService
    {
        private readonly LotRepository _repository;

        public LotService(LotRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Lot>> ObtenerLotesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddLotAsync(Lot lot)
        {
            await _repository.AddAsync(lot);
        }

        public async Task CrearLoteAsync(int codigo, bool activo)
        {
            var lote = new Lot
            {
                Code = codigo,
                IsActive = activo,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(lote);
        }
   
    }
}
