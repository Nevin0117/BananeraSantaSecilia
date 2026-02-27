using Microsoft.EntityFrameworkCore;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Data;

namespace SantaSecilia.Infrastructure.Repositories;

public class DailyRecordRepository(IDbContextFactory<AppDbContext> contextFactory)
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public async Task<DailyRecord?> GetByWorkerAndDateAsync(int workerId, DateTime date)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.DailyRecords
            .Include(r => r.Lines)
                .ThenInclude(l => l.Activity)
            .Include(r => r.Lines)
                .ThenInclude(l => l.Lot)
            .FirstOrDefaultAsync(r => r.WorkerId == workerId && r.WorkDate.Date == date.Date);
    }

    public async Task AddAsync(DailyRecord record)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.DailyRecords.Add(record);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DailyRecord record)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.DailyRecords.Update(record);
        await context.SaveChangesAsync();
    }

    public async Task SaveLineAsync(DailyRecordLine line)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        if (line.Id == 0)
            context.DailyRecordLines.Add(line);
        else
            context.DailyRecordLines.Update(line);

        await context.SaveChangesAsync();
    }
}
