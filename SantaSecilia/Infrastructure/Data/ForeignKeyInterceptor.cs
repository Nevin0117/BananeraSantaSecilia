using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace SantaSecilia.Infrastructure.Data;

/// <summary>Habilita PRAGMA foreign_keys=ON en cada conexión SQLite.</summary>
public sealed class ForeignKeyInterceptor : DbConnectionInterceptor
{
    private static void EnableFk(DbConnection connection)
    {
        if (connection is not SqliteConnection sqlite) return;
        using var cmd = sqlite.CreateCommand();
        cmd.CommandText = "PRAGMA foreign_keys=ON;";
        cmd.ExecuteNonQuery();
    }

    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        => EnableFk(connection);

    public override Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData,
        CancellationToken cancellationToken = default)
    {
        EnableFk(connection);
        return Task.CompletedTask;
    }
}
