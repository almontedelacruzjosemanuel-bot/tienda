using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class CiudadServiceTests : SqliteServiceTestBase
{
    private readonly CiudadService _service;

    public CiudadServiceTests()
    {
        _service = new CiudadService(Context);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoNoHayCiudades_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodas();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoExistenCiudades_RetornaTodasMapeadas()
    {
        Context.Ciudades.AddRange(
            new Ciudad { Nombre = "Bonao" },
            new Ciudad { Nombre = "Santiago" }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodas();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, c => c.Nombre == "Bonao");
        Assert.Contains(resultado, c => c.Nombre == "Santiago");
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaCiudadDto()
    {
        var entidad = new Ciudad { Nombre = "La Vega" };
        Context.Ciudades.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.CiudadId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.CiudadId, resultado!.CiudadId);
        Assert.Equal("La Vega", resultado.Nombre);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_CiudadValida_GuardaEnBaseYAsignaId()
    {
        var dto = new CiudadDto { Nombre = "Santo Domingo" };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.CiudadId > 0);
        Assert.Equal("Santo Domingo", resultado.Nombre);
        Assert.Equal(1, await Context.Ciudades.CountAsync());
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaNombreYRetornaTrue()
    {
        var entidad = new Ciudad { Nombre = "Nombre Viejo" };
        Context.Ciudades.Add(entidad);
        await Context.SaveChangesAsync();

        var dtoActualizado = new CiudadDto
        {
            CiudadId = entidad.CiudadId,
            Nombre = "Nombre Nuevo"
        };

        var actualizado = await _service.Actualizar(entidad.CiudadId, dtoActualizado);
        var enDb = await Context.Ciudades.FindAsync(entidad.CiudadId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal("Nombre Nuevo", enDb!.Nombre);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new CiudadDto { Nombre = "Inexistente" };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var entidad = new Ciudad { Nombre = "Temporal" };
        Context.Ciudades.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.CiudadId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.Ciudades.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
