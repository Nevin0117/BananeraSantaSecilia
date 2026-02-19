using Microsoft.EntityFrameworkCore;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Data;

namespace SantaSecilia.Infrastructure.Repositories;

public class LotRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public LotRepository(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Lot>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Set<Lot>()
            .OrderBy(l => l.Code)
            .ToListAsync();
    }

    public async Task AddAsync(Lot lot)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        context.Add(lot);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Lot lot)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        context.Update(lot);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByCodeAsync(int code)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<Lot>()
            .AnyAsync(l => l.Code == code);
    }
}
