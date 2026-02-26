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

    // Obtener todos los lotes ordenados por código
    public async Task<List<Lot>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Set<Lot>()
            .OrderBy(l => l.Code)
            .ToListAsync();
    }

    // Agregar lote a la BD
    public async Task AddAsync(Lot lot)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        context.Add(lot);
        await context.SaveChangesAsync();
    }

    // Actualizar un lote en la BD
    public async Task UpdateAsync(Lot lot)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        context.Update(lot);
        await context.SaveChangesAsync();
    }

    // Obtener lote por su identificador
    public async Task<Lot?> GetByIdAsync(int id)
    {   await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<Lot>()
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    // Vereficar si ya existe un lote con el mismo código
    public async Task<bool> ExistsByCodeAsync(int code, int? ignoreId = null)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Set<Lot>()
            .AnyAsync(l => l.Code == code && (ignoreId == null || l.Id != ignoreId));
    }

    // Verificar si existe un lote con el código dado (sin ignorar ningún ID)
    public async Task<bool> ExistsByCodeAsync(int code)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<Lot>()
            .AnyAsync(l => l.Code == code);
    }

    // Eliminar un lote por su identificador
    public async Task DeleteAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var lote = await context.Set<Lot>()
                                .FirstOrDefaultAsync(l => l.Id == id);

        if (lote != null)
        {
            context.Remove(lote);
            await context.SaveChangesAsync();
        }
    }

}
