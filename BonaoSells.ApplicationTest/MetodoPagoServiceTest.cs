using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class MetodoPagoServiceTests : SqliteServiceTestBase
{
    private readonly MetodoPagoService _service;

    public MetodoPagoServiceTests()
    {
        _service = new MetodoPagoService(Context);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoNoHayMetodosPago_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodos();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoExistenMetodosPago_RetornaTodosMapeados()
    {
        Context.MetodoPagos.AddRange(
            new MetodoPago { Nombre = "Efectivo" },
            new MetodoPago { Nombre = "Tarjeta de Crédito" }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodos();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, m => m.Nombre == "Efectivo");
        Assert.Contains(resultado, m => m.Nombre == "Tarjeta de Crédito");
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaMetodoPagoDto()
    {
        var entidad = new MetodoPago { Nombre = "Transferencia" };
        Context.MetodoPagos.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.MetodoPagoId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.MetodoPagoId, resultado!.MetodoPagoId);
        Assert.Equal("Transferencia", resultado.Nombre);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_MetodoPagoValido_GuardaEnBaseYAsignaId()
    {
        var dto = new MetodoPagoDto { Nombre = "Cheque" };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.MetodoPagoId > 0);
        Assert.Equal("Cheque", resultado.Nombre);
        Assert.Equal(1, await Context.MetodoPagos.CountAsync());
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaNombreYRetornaTrue()
    {
        var entidad = new MetodoPago { Nombre = "Efectivo" };
        Context.MetodoPagos.Add(entidad);
        await Context.SaveChangesAsync();

        var dtoActualizado = new MetodoPagoDto
        {
            MetodoPagoId = entidad.MetodoPagoId,
            Nombre = "Efectivo DOP"
        };

        var actualizado = await _service.Actualizar(entidad.MetodoPagoId, dtoActualizado);
        var enDb = await Context.MetodoPagos.FindAsync(entidad.MetodoPagoId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal("Efectivo DOP", enDb!.Nombre);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new MetodoPagoDto { Nombre = "No existe" };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var entidad = new MetodoPago { Nombre = "Temporal" };
        Context.MetodoPagos.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.MetodoPagoId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.MetodoPagos.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
