using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class CompraDetalleServiceTests : SqliteServiceTestBase
{
    private readonly CompraDetalleService _service;

    public CompraDetalleServiceTests()
    {
        _service = new CompraDetalleService(Context);
    }

    private async Task<(int CompraId, int ProductoId)> SembrarDependenciasAsync()
    {
        var proveedor = new Proveedor { NombreEmpresa = "Suplidora Norte" };
        var empleado = new Empleado
        {
            Nombre = "Carlos",
            Apellido = "Pérez",
            FechaContratacion = new DateOnly(2026, 1, 1)
        };
        var categoria = new Categoria { Nombre = "Audio" };
        var marca = new Marca { Nombre = "Sony" };

        Context.Proveedores.Add(proveedor);
        Context.Empleados.Add(empleado);
        Context.Categorias.Add(categoria);
        Context.Marcas.Add(marca);
        await Context.SaveChangesAsync();

        var compra = new Compra
        {
            ProveedorId = proveedor.ProveedorId,
            EmpleadoId = empleado.EmpleadoId,
            Fecha = new DateTime(2026, 11, 27, 9, 0, 0),
            Total = 10000m,
            Estado = "Recibida"
        };
        var producto = new Producto
        {
            CategoriaId = categoria.CategoriaId,
            MarcaId = marca.MarcaId,
            Nombre = "Audífonos",
            Costo = 1000m,
            Precio = 1600m
        };

        Context.Compras.Add(compra);
        Context.Productos.Add(producto);
        await Context.SaveChangesAsync();

        return (compra.CompraId, producto.ProductoId);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoNoHayDetalles_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodos();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoExistenDetalles_RetornaTodosMapeados()
    {
        var (compraId, productoId) = await SembrarDependenciasAsync();

        Context.CompraDetalles.AddRange(
            new CompraDetalle
            {
                CompraId = compraId,
                ProductoId = productoId,
                Cantidad = 5,
                CostoUnitario = 1000m,
                Subtotal = 5000m
            },
            new CompraDetalle
            {
                CompraId = compraId,
                ProductoId = productoId,
                Cantidad = 3,
                CostoUnitario = 1000m,
                Subtotal = 3000m
            }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodos();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, d => d.Cantidad == 5 && d.Subtotal == 5000m);
        Assert.Contains(resultado, d => d.Cantidad == 3 && d.Subtotal == 3000m);
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaCompraDetalleDto()
    {
        var (compraId, productoId) = await SembrarDependenciasAsync();
        var entidad = new CompraDetalle
        {
            CompraId = compraId,
            ProductoId = productoId,
            Cantidad = 10,
            CostoUnitario = 950m,
            Subtotal = 9500m
        };
        Context.CompraDetalles.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.CompraDetalleId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.CompraDetalleId, resultado!.CompraDetalleId);
        Assert.Equal(compraId, resultado.CompraId);
        Assert.Equal(productoId, resultado.ProductoId);
        Assert.Equal(10, resultado.Cantidad);
        Assert.Equal(950m, resultado.CostoUnitario);
        Assert.Equal(9500m, resultado.Subtotal);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_DetalleValido_GuardaEnBaseYAsignaId()
    {
        var (compraId, productoId) = await SembrarDependenciasAsync();
        var dto = new CompraDetalleDto
        {
            CompraId = compraId,
            ProductoId = productoId,
            Cantidad = 4,
            CostoUnitario = 1200m,
            Subtotal = 4800m
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.CompraDetalleId > 0);
        Assert.Equal(4800m, resultado.Subtotal);
        Assert.Equal(1, await Context.CompraDetalles.CountAsync());
    }

    [Fact]
    public async Task Crear_CompraOProductoInexistente_LanzaDbUpdateException()
    {
        var dto = new CompraDetalleDto
        {
            CompraId = 9999,
            ProductoId = 9999,
            Cantidad = 1,
            CostoUnitario = 100m,
            Subtotal = 100m
        };

        await Assert.ThrowsAsync<DbUpdateException>(() => _service.Crear(dto));
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaCamposYRetornaTrue()
    {
        var (compra1, producto1) = await SembrarDependenciasAsync();
        var (compra2, producto2) = await SembrarDependenciasAsync();

        var entidad = new CompraDetalle
        {
            CompraId = compra1,
            ProductoId = producto1,
            Cantidad = 2,
            CostoUnitario = 500m,
            Subtotal = 1000m
        };
        Context.CompraDetalles.Add(entidad);
        await Context.SaveChangesAsync();

        var dtoActualizado = new CompraDetalleDto
        {
            CompraDetalleId = entidad.CompraDetalleId,
            CompraId = compra2,
            ProductoId = producto2,
            Cantidad = 6,
            CostoUnitario = 750m,
            Subtotal = 4500m
        };

        var actualizado = await _service.Actualizar(entidad.CompraDetalleId, dtoActualizado);
        var enDb = await Context.CompraDetalles.FindAsync(entidad.CompraDetalleId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal(compra2, enDb!.CompraId);
        Assert.Equal(producto2, enDb.ProductoId);
        Assert.Equal(6, enDb.Cantidad);
        Assert.Equal(750m, enDb.CostoUnitario);
        Assert.Equal(4500m, enDb.Subtotal);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new CompraDetalleDto { Cantidad = 1, CostoUnitario = 10m, Subtotal = 10m };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var (compraId, productoId) = await SembrarDependenciasAsync();
        var entidad = new CompraDetalle
        {
            CompraId = compraId,
            ProductoId = productoId,
            Cantidad = 1,
            CostoUnitario = 100m,
            Subtotal = 100m
        };
        Context.CompraDetalles.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.CompraDetalleId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.CompraDetalles.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
