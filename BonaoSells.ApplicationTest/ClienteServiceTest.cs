using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class ClienteServiceTests : SqliteServiceTestBase
{
    private readonly ClienteService _service;

    public ClienteServiceTests()
    {
        _service = new ClienteService(Context);
    }

    private async Task<int> SembrarCiudadAsync(string nombre = "Bonao")
    {
        var ciudad = new Ciudad { Nombre = nombre };
        Context.Ciudades.Add(ciudad);
        await Context.SaveChangesAsync();
        return ciudad.CiudadId;
    }

    [Fact]
    public async Task ObtenerTodos_CuandoNoHayClientes_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodos();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoExistenClientes_RetornaTodosMapeados()
    {
        var ciudadId = await SembrarCiudadAsync();
        var fecha = new DateOnly(2026, 11, 20);

        Context.Clientes.AddRange(
            new Cliente
            {
                NombreCompleto = "Juan Pérez",
                DocumentoIdentidad = "001-0000001-1",
                Email = "juan@mail.com",
                Telefono = "809-555-0001",
                Direccion = "Calle 1",
                FechaRegistro = fecha,
                CiudadId = ciudadId
            },
            new Cliente
            {
                NombreCompleto = "María García",
                DocumentoIdentidad = "001-0000002-2",
                FechaRegistro = fecha,
                CiudadId = null
            }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodos();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, c => c.NombreCompleto == "Juan Pérez" && c.CiudadId == ciudadId);
        Assert.Contains(resultado, c => c.NombreCompleto == "María García" && c.CiudadId == null);
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaClienteDto()
    {
        var ciudadId = await SembrarCiudadAsync("La Vega");
        var fecha = new DateOnly(2026, 10, 5);

        var entidad = new Cliente
        {
            NombreCompleto = "Pedro Ramírez",
            DocumentoIdentidad = "047-0000003-3",
            Email = "pedro@mail.com",
            Telefono = "809-555-0003",
            Direccion = "Av. Principal #12",
            FechaRegistro = fecha,
            CiudadId = ciudadId
        };
        Context.Clientes.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.ClienteId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.ClienteId, resultado!.ClienteId);
        Assert.Equal("Pedro Ramírez", resultado.NombreCompleto);
        Assert.Equal("047-0000003-3", resultado.DocumentoIdentidad);
        Assert.Equal("pedro@mail.com", resultado.Email);
        Assert.Equal("809-555-0003", resultado.Telefono);
        Assert.Equal("Av. Principal #12", resultado.Direccion);
        Assert.Equal(fecha, resultado.FechaRegistro);
        Assert.Equal(ciudadId, resultado.CiudadId);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_ClienteValidoConCiudad_GuardaEnBaseYAsignaId()
    {
        var ciudadId = await SembrarCiudadAsync();
        var dto = new ClienteDto
        {
            NombreCompleto = "Laura Méndez",
            DocumentoIdentidad = "048-0000004-4",
            Email = "laura@mail.com",
            Telefono = "829-555-0004",
            Direccion = "Centro",
            FechaRegistro = new DateOnly(2026, 11, 27),
            CiudadId = ciudadId
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.ClienteId > 0);
        Assert.Equal("Laura Méndez", resultado.NombreCompleto);
        Assert.Equal(1, await Context.Clientes.CountAsync());
    }

    [Fact]
    public async Task Crear_CiudadInexistente_LanzaDbUpdateException()
    {
        var dto = new ClienteDto
        {
            NombreCompleto = "Cliente Inválido",
            CiudadId = 9999
        };

        await Assert.ThrowsAsync<DbUpdateException>(() => _service.Crear(dto));
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaCamposYRetornaTrue()
    {
        var ciudadInicial = await SembrarCiudadAsync("Bonao");
        var ciudadNueva = await SembrarCiudadAsync("Santiago");
        var entidad = new Cliente
        {
            NombreCompleto = "Nombre Viejo",
            DocumentoIdentidad = "000-0000000-0",
            Email = "viejo@mail.com",
            Telefono = "809-000-0000",
            Direccion = "Vieja",
            FechaRegistro = new DateOnly(2025, 1, 1),
            CiudadId = ciudadInicial
        };
        Context.Clientes.Add(entidad);
        await Context.SaveChangesAsync();

        var nuevaFecha = new DateOnly(2026, 2, 2);
        var dtoActualizado = new ClienteDto
        {
            ClienteId = entidad.ClienteId,
            NombreCompleto = "Nombre Nuevo",
            DocumentoIdentidad = "001-1111111-1",
            Email = "nuevo@mail.com",
            Telefono = "809-111-1111",
            Direccion = "Nueva",
            FechaRegistro = nuevaFecha,
            CiudadId = ciudadNueva
        };

        var actualizado = await _service.Actualizar(entidad.ClienteId, dtoActualizado);
        var enDb = await Context.Clientes.FindAsync(entidad.ClienteId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal("Nombre Nuevo", enDb!.NombreCompleto);
        Assert.Equal("001-1111111-1", enDb.DocumentoIdentidad);
        Assert.Equal("nuevo@mail.com", enDb.Email);
        Assert.Equal("809-111-1111", enDb.Telefono);
        Assert.Equal("Nueva", enDb.Direccion);
        Assert.Equal(nuevaFecha, enDb.FechaRegistro);
        Assert.Equal(ciudadNueva, enDb.CiudadId);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new ClienteDto { NombreCompleto = "No existe" };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var entidad = new Cliente { NombreCompleto = "Temporal" };
        Context.Clientes.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.ClienteId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.Clientes.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
