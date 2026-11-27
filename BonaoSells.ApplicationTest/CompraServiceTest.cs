using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class CompraServiceTests : SqliteServiceTestBase
{
    private readonly CompraService _service;

    public CompraServiceTests()
    {
        _service = new CompraService(Context);
    }

    private async Task<(int ProveedorId, int EmpleadoId)> SembrarCatalogosAsync()
    {
        var proveedor = new Proveedor
        {
            NombreEmpresa = "TechImport SRL",
            ContactoNombre = "Carlos Ruiz"
        };
        var empleado = new Empleado
        {
            Nombre = "Ana",
            Apellido = "Gómez",
            Cargo = "Compradora",
            FechaContratacion = new DateOnly(2026, 1, 1)
        };

        Context.Proveedores.Add(proveedor);
        Context.Empleados.Add(empleado);
        await Context.SaveChangesAsync();

        return (proveedor.ProveedorId, empleado.EmpleadoId);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoNoHayCompras_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodas();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoExistenCompras_RetornaTodasMapeadas()
    {
        var (proveedorId, empleadoId) = await SembrarCatalogosAsync();
        var fecha = new DateTime(2026, 11, 26, 9, 30, 0);

        Context.Compras.AddRange(
            new Compra
            {
                ProveedorId = proveedorId,
                EmpleadoId = empleadoId,
                Fecha = fecha,
                Total = 15000m,
                Estado = "Recibida"
            },
            new Compra
            {
                ProveedorId = proveedorId,
                EmpleadoId = empleadoId,
                Fecha = fecha.AddDays(1),
                Total = 8500m,
                Estado = "Pendiente"
            }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodas();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, c => c.Total == 15000m && c.Estado == "Recibida");
        Assert.Contains(resultado, c => c.Total == 8500m && c.Estado == "Pendiente");
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaCompraDto()
    {
        var (proveedorId, empleadoId) = await SembrarCatalogosAsync();
        var fecha = new DateTime(2026, 11, 25, 14, 0, 0);

        var entidad = new Compra
        {
            ProveedorId = proveedorId,
            EmpleadoId = empleadoId,
            Fecha = fecha,
            Total = 22500m,
            Estado = "Completada"
        };
        Context.Compras.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.CompraId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.CompraId, resultado!.CompraId);
        Assert.Equal(proveedorId, resultado.ProveedorId);
        Assert.Equal(empleadoId, resultado.EmpleadoId);
        Assert.Equal(fecha, resultado.Fecha);
        Assert.Equal(22500m, resultado.Total);
        Assert.Equal("Completada", resultado.Estado);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_CompraValida_GuardaEnBaseYAsignaId()
    {
        var (proveedorId, empleadoId) = await SembrarCatalogosAsync();
        var dto = new CompraDto
        {
            ProveedorId = proveedorId,
            EmpleadoId = empleadoId,
            Fecha = new DateTime(2026, 11, 27, 10, 0, 0),
            Total = 12300m,
            Estado = "Pendiente"
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.CompraId > 0);
        Assert.Equal(12300m, resultado.Total);
        Assert.Equal(1, await Context.Compras.CountAsync());
    }

    [Fact]
    public async Task Crear_ProveedorOEmpleadoInexistente_LanzaDbUpdateException()
    {
        var dto = new CompraDto
        {
            ProveedorId = 9999,
            EmpleadoId = 9999,
            Fecha = DateTime.UtcNow,
            Total = 500m,
            Estado = "Pendiente"
        };

        await Assert.ThrowsAsync<DbUpdateException>(() => _service.Crear(dto));
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaCamposYRetornaTrue()
    {
        var (proveedor1, empleado1) = await SembrarCatalogosAsync();
        var (proveedor2, empleado2) = await SembrarCatalogosAsync();

        var entidad = new Compra
        {
            ProveedorId = proveedor1,
            EmpleadoId = empleado1,
            Fecha = new DateTime(2026, 11, 20, 8, 0, 0),
            Total = 1000m,
            Estado = "Pendiente"
        };
        Context.Compras.Add(entidad);
        await Context.SaveChangesAsync();

        var nuevaFecha = new DateTime(2026, 11, 27, 12, 0, 0);
        var dtoActualizado = new CompraDto
        {
            CompraId = entidad.CompraId,
            ProveedorId = proveedor2,
            EmpleadoId = empleado2,
            Fecha = nuevaFecha,
            Total = 3400m,
            Estado = "Recibida"
        };

        var actualizado = await _service.Actualizar(entidad.CompraId, dtoActualizado);
        var enDb = await Context.Compras.FindAsync(entidad.CompraId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal(proveedor2, enDb!.ProveedorId);
        Assert.Equal(empleado2, enDb.EmpleadoId);
        Assert.Equal(nuevaFecha, enDb.Fecha);
        Assert.Equal(3400m, enDb.Total);
        Assert.Equal("Recibida", enDb.Estado);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new CompraDto { Total = 100m, Estado = "Pendiente" };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var (proveedorId, empleadoId) = await SembrarCatalogosAsync();
        var entidad = new Compra
        {
            ProveedorId = proveedorId,
            EmpleadoId = empleadoId,
            Fecha = DateTime.UtcNow,
            Total = 500m,
            Estado = "Cancelada"
        };
        Context.Compras.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.CompraId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.Compras.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
