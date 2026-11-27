using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using BonaoSells.ApplicationTest.Helpers;

namespace BonaoSells.ApplicationTest.Services;

public class CategoriaServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly BonaoSellsContext _context;
    private readonly CategoriaService _service;

    public CategoriaServiceTests()
    {
        // "DataSource=:memory:" requiere mantener la conexión abierta durante el test
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<BonaoSellsContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new TestBonaoSellsContext(options);
        _context.Database.EnsureCreated();

        _service = new CategoriaService(_context);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoNoHayCategorias_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodas();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoExistenCategorias_RetornaTodasMapeadas()
    {
        _context.Categorias.AddRange(
            new Categoria { Nombre = "Bebidas", Descripcion = "Refrescos y jugos" },
            new Categoria { Nombre = "Granos", Descripcion = "Arroz, habichuelas y más" }
        );
        await _context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodas();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, c => c.Nombre == "Bebidas" && c.Descripcion == "Refrescos y jugos");
        Assert.Contains(resultado, c => c.Nombre == "Granos");
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaCategoriaDto()
    {
        var entidad = new Categoria { Nombre = "Lácteos", Descripcion = "Leche y queso" };
        _context.Categorias.Add(entidad);
        await _context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.CategoriaId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.CategoriaId, resultado!.CategoriaId);
        Assert.Equal("Lácteos", resultado.Nombre);
        Assert.Equal("Leche y queso", resultado.Descripcion);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_CategoriaValida_GuardaEnBaseYAsignaId()
    {
        var dto = new CategoriaDto
        {
            Nombre = "Limpieza",
            Descripcion = "Productos del hogar"
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.CategoriaId > 0);
        Assert.Equal("Limpieza", resultado.Nombre);
        Assert.Equal(1, await _context.Categorias.CountAsync());
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaDatosYRetornaTrue()
    {
        var entidad = new Categoria { Nombre = "Viejo", Descripcion = "Desc vieja" };
        _context.Categorias.Add(entidad);
        await _context.SaveChangesAsync();

        var dtoActualizado = new CategoriaDto
        {
            CategoriaId = entidad.CategoriaId,
            Nombre = "Nuevo",
            Descripcion = "Desc nueva"
        };

        var actualizado = await _service.Actualizar(entidad.CategoriaId, dtoActualizado);
        var enDb = await _context.Categorias.FindAsync(entidad.CategoriaId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal("Nuevo", enDb!.Nombre);
        Assert.Equal("Desc nueva", enDb.Descripcion);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new CategoriaDto { Nombre = "No existe", Descripcion = "N/A" };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var entidad = new Categoria { Nombre = "Temporal", Descripcion = "Borrar" };
        _context.Categorias.Add(entidad);
        await _context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.CategoriaId);

        Assert.True(eliminado);
        Assert.Equal(0, await _context.Categorias.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}