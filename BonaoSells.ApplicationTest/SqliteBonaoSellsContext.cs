using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Helpers;

public class TestBonaoSellsContext : BonaoSellsContext
{
    public TestBonaoSellsContext(DbContextOptions<BonaoSellsContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (Database.IsSqlite())
        {
            modelBuilder.Entity<Producto>()
                .Property(e => e.Descripcion)
                .HasColumnType("TEXT");
        }
    }
}