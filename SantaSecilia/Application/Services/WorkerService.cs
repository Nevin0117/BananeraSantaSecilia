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

        public Task<List<Worker>> BuscarTrabajadoresAsync(string query)
        {
            return _workerRepository.SearchAsync(query);
        }

        public async Task<(bool success, string message)> EliminarTrabajadorAsync(int id)
        {
            try
            {
                // Validación de integridad relacional
                var tieneRegistros = await _workerRepository.TieneRegistrosRelacionadosAsync(id);

                if (tieneRegistros)
                {
                    return (false, "Este trabajador no puede ser eliminado porque tiene registros históricos de labores. Se recomienda desactivarlo desde 'Editar' en su lugar.");
                }

                await _workerRepository.DeleteAsync(id);
                return (true, "Trabajador eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error técnico al intentar eliminar: {ex.Message}");
            }
        }

        public async Task<bool> ExisteTrabajadorConCedulaAsync(string cedula, int? ignorarId = null)
        {
            return await _workerRepository.ExistsByIdentificationAsync(cedula, ignorarId);
        }
    }
}
