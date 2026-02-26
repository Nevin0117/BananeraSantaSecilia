using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Repositories;

namespace SantaSecilia.Application.Services
{
    public class WorkerService{
        private readonly WorkerRepository _workerRepository;

        public WorkerService(WorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public Task<List<Worker>> ObtenerTrabajadoresAsync()
        {
            return _workerRepository.GetAllAsync();
        }

        public Task<Worker?> ObtenerPorIdAsync(int id)
        {
            return _workerRepository.GetByIdAsync(id);
        }

        public Task AgregarTrabajadorAsync(Worker worker)
        {
            return _workerRepository.AddAsync(worker);
        }

        public Task ActualizarTrabajadorAsync(Worker worker)
        {
            return _workerRepository.UpdateAsync(worker);
        }
    }
}
