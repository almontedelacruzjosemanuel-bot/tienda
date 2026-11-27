using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class ProveedorServiceTests : SqliteServiceTestBase
{
    private readonly ProveedorService _service;

    public ProveedorServiceTests()
    {
        _service = new ProveedorService(Context);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoNoHayProveedores_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodos();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoExistenProveedores_RetornaTodosMapeados()
    {
        Context.Proveedores.AddRange(
            new Proveedor
            {
                NombreEmpresa = "TechImport SRL",
                ContactoNombre = "Carlos Ruiz",
                Telefono = "809-555-0101",
                Email = "carlos@techimport.do",
                Direccion = "Av. Duarte #10"
            },
            new Proveedor
            {
                NombreEmpresa = "ElectroBonao",
                ContactoNombre = "María López",
                Telefono = "809-555-0202",
                Email = "maria@electrobonao.do",
                Direccion = "Calle Libertad #45"
            }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodos();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, p => p.NombreEmpresa == "TechImport SRL" && p.ContactoNombre == "Carlos Ruiz");
        Assert.Contains(resultado, p => p.NombreEmpresa == "ElectroBonao" && p.Email == "maria@electrobonao.do");
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaProveedorDto()
    {
        var entidad = new Proveedor
        {
            NombreEmpresa = "Distribuidora Cibao",
            ContactoNombre = "Pedro Núñez",
            Telefono = "809-555-0303",
            Email = "ventas@cibao.do",
            Direccion = "Autopista Duarte Km 85"
        };
        Context.Proveedores.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.ProveedorId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.ProveedorId, resultado!.ProveedorId);
        Assert.Equal("Distribuidora Cibao", resultado.NombreEmpresa);
        Assert.Equal("Pedro Núñez", resultado.ContactoNombre);
        Assert.Equal("809-555-0303", resultado.Telefono);
        Assert.Equal("ventas@cibao.do", resultado.Email);
        Assert.Equal("Autopista Duarte Km 85", resultado.Direccion);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_ProveedorValido_GuardaEnBaseYAsignaId()
    {
        var dto = new ProveedorDto
        {
            NombreEmpresa = "Global Parts",
            ContactoNombre = "Luis Ortiz",
            Telefono = "829-555-0404",
            Email = "luis@globalparts.do",
            Direccion = "Zona Industrial"
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.ProveedorId > 0);
        Assert.Equal("Global Parts", resultado.NombreEmpresa);
        Assert.Equal(1, await Context.Proveedores.CountAsync());
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaDatosYRetornaTrue()
    {
        var entidad = new Proveedor
        {
            NombreEmpresa = "Empresa Vieja",
            ContactoNombre = "Contacto Viejo",
            Telefono = "809-000-0000",
            Email = "viejo@test.do",
            Direccion = "Dir vieja"
        };
        Context.Proveedores.Add(entidad);
        await Context.SaveChangesAsync();

        var dtoActualizado = new ProveedorDto
        {
            ProveedorId = entidad.ProveedorId,
            NombreEmpresa = "Empresa Nueva",
            ContactoNombre = "Contacto Nuevo",
            Telefono = "809-999-9999",
            Email = "nuevo@test.do",
            Direccion = "Dir nueva"
        };

        var actualizado = await _service.Actualizar(entidad.ProveedorId, dtoActualizado);
        var enDb = await Context.Proveedores.FindAsync(entidad.ProveedorId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal("Empresa Nueva", enDb!.NombreEmpresa);
        Assert.Equal("Contacto Nuevo", enDb.ContactoNombre);
        Assert.Equal("809-999-9999", enDb.Telefono);
        Assert.Equal("nuevo@test.do", enDb.Email);
        Assert.Equal("Dir nueva", enDb.Direccion);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new ProveedorDto { NombreEmpresa = "No existe" };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var entidad = new Proveedor { NombreEmpresa = "Temporal" };
        Context.Proveedores.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.ProveedorId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.Proveedores.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
