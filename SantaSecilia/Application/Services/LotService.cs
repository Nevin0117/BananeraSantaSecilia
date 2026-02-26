using System;
using System.Collections.Generic;
using System.Text;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Repositories;

namespace SantaSecilia.Application.Services
{
    // Servicio para manejar la lógica de negocio relacionada con los lotes, utilizando el repositorio para acceder a los datos
    public class LotService
    {
        private readonly LotRepository _repository;

        // Inyectar el repositorio de lotes para acceder a los datos
        public LotService(LotRepository repository)
        {
            _repository = repository;
        }

        // Obtener todos los lotes de la BD de forma asíncrona
        public async Task<List<Lot>> ObtenerLotesAsync()
        {
            return await _repository.GetAllAsync(); 
        }

        // Agregar un nuevo lote a la BD de forma asíncrona
        public async Task AddLotAsync(Lot lot)
        {
            await _repository.AddAsync(lot);
        }

        // Crear un nuevo lote con los datos proporcionados y agregarlo a la BD de forma asíncrona
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
