using Microsoft.EntityFrameworkCore;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Data;

namespace SantaSecilia.Infrastructure.Repositories;

public class UserRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public UserRepository(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(User user)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByUsernameAsync(string username, int? ignoreId = null)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<User>()
            .AnyAsync(u => u.Username == username && (ignoreId == null || u.Id != ignoreId));
    }
}
