using BonaoSells.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Helpers;

/// <summary>
/// Base común para pruebas unitarias de servicios usando SQLite en memoria con claves foráneas habilitadas.
/// </summary>
public abstract class SqliteServiceTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly BonaoSellsContext Context;

    protected SqliteServiceTestBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        using (var pragmaCmd = _connection.CreateCommand())
        {
            pragmaCmd.CommandText = "PRAGMA foreign_keys = ON;";
            pragmaCmd.ExecuteNonQuery();
        }

        var options = new DbContextOptionsBuilder<BonaoSellsContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new TestBonaoSellsContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}
