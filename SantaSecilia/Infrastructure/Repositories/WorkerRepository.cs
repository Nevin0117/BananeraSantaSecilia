using Microsoft.EntityFrameworkCore;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Data;

namespace SantaSecilia.Infrastructure.Repositories
{
    public class WorkerRepository{
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public WorkerRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Worker>> GetAllAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.Set<Worker>()
                .OrderBy(w => w.FullName)
                .ToListAsync();
        }

        public async Task<Worker?> GetByIdAsync(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.Set<Worker>()
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddAsync(Worker worker)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            context.Add(worker);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Worker worker)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            context.Update(worker);
            await context.SaveChangesAsync();
        }

        public async Task<List<Worker>> SearchAsync(string query)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Worker>()
                .Where(w => w.IsActive &&
                           (w.FullName.Contains(query) || w.IdentificationNumber.Contains(query)))
                .OrderBy(w => w.FullName)
                .ToListAsync();
        }
    }
}