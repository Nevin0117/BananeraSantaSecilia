using Microsoft.EntityFrameworkCore;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Data;

namespace SantaSecilia.Infrastructure.Repositories;

public class ActivityRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ActivityRepository(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Activity>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<Activity>()
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<List<Activity>> GetActiveAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<Activity>()
            .Where(a => a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Activity activity)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Add(activity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Activity activity)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Update(activity);
        await context.SaveChangesAsync();
    }

    public async Task<Activity?> GetByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<Activity>()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> ExistsByNameAsync(string name, int? ignoreId = null)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<Activity>()
            .AnyAsync(a => a.Name == name && (ignoreId == null || a.Id != ignoreId));
    }
}
