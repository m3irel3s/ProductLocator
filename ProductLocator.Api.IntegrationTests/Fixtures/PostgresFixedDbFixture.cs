using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Xunit;
using ProductLocator.Api.Data;

public sealed class PostgresFixedDbFixture : IAsyncLifetime
{
    public string ConnectionString =>
        "Host=127.0.0.1;Port=5433;Database=productlocator_test;Username=productlocator;Password=productlocator";

    private Respawner _respawner = null!;

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        await using (var db = new AppDbContext(options))
        {
            await db.Database.MigrateAsync();
        }

        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
            WithReseed = true,
            TablesToIgnore = new[]
            {
                new Respawn.Graph.Table("public", "__EFMigrationsHistory")
            }
        });

    }

    public async Task ResetAsync()
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();
        await _respawner.ResetAsync(conn);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
