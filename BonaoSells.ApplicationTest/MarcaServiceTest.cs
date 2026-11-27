using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class MarcaServiceTests : SqliteServiceTestBase
{
    private readonly MarcaService _service;

    public MarcaServiceTests()
    {
        _service = new MarcaService(Context);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoNoHayMarcas_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodas();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoExistenMarcas_RetornaTodasMapeadas()
    {
        Context.Marcas.AddRange(
            new Marca { Nombre = "Samsung", PaisOrigen = "Corea del Sur", SitioWeb = "https://samsung.com" },
            new Marca { Nombre = "Apple", PaisOrigen = "Estados Unidos", SitioWeb = "https://apple.com" }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodas();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, m => m.Nombre == "Samsung" && m.PaisOrigen == "Corea del Sur");
        Assert.Contains(resultado, m => m.Nombre == "Apple" && m.SitioWeb == "https://apple.com");
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaMarcaDto()
    {
        var entidad = new Marca
        {
            Nombre = "Dell",
            PaisOrigen = "Estados Unidos",
            SitioWeb = "https://dell.com"
        };
        Context.Marcas.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.MarcaId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.MarcaId, resultado!.MarcaId);
        Assert.Equal("Dell", resultado.Nombre);
        Assert.Equal("Estados Unidos", resultado.PaisOrigen);
        Assert.Equal("https://dell.com", resultado.SitioWeb);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_MarcaValida_GuardaEnBaseYAsignaId()
    {
        var dto = new MarcaDto
        {
            Nombre = "Lenovo",
            PaisOrigen = "China",
            SitioWeb = "https://lenovo.com"
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.MarcaId > 0);
        Assert.Equal("Lenovo", resultado.Nombre);
        Assert.Equal(1, await Context.Marcas.CountAsync());
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaDatosYRetornaTrue()
    {
        var entidad = new Marca
        {
            Nombre = "Marca Vieja",
            PaisOrigen = "País Viejo",
            SitioWeb = "https://viejo.com"
        };
        Context.Marcas.Add(entidad);
        await Context.SaveChangesAsync();

        var dtoActualizado = new MarcaDto
        {
            MarcaId = entidad.MarcaId,
            Nombre = "Marca Nueva",
            PaisOrigen = "Japón",
            SitioWeb = "https://nuevo.com"
        };

        var actualizado = await _service.Actualizar(entidad.MarcaId, dtoActualizado);
        var enDb = await Context.Marcas.FindAsync(entidad.MarcaId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal("Marca Nueva", enDb!.Nombre);
        Assert.Equal("Japón", enDb.PaisOrigen);
        Assert.Equal("https://nuevo.com", enDb.SitioWeb);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new MarcaDto { Nombre = "Inexistente" };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var entidad = new Marca { Nombre = "Temporal" };
        Context.Marcas.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.MarcaId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.Marcas.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
