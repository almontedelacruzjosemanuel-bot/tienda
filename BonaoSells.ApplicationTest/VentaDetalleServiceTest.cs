using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class VentaDetalleServiceTests : SqliteServiceTestBase
{
    private readonly VentaDetalleService _service;

    public VentaDetalleServiceTests()
    {
        _service = new VentaDetalleService(Context);
    }

    private async Task<(int VentaId, int ProductoId)> SembrarDependenciasAsync()
    {
        var fechaHoy = DateOnly.FromDateTime(DateTime.Today);
        var cliente = new Cliente
        {
            NombreCompleto = "Juan Pérez",
            FechaRegistro = fechaHoy
        };
        var empleado = new Empleado
        {
            Nombre = "Ana",
            Apellido = "Gómez",
            FechaContratacion = fechaHoy
        };
        var metodoPago = new MetodoPago { Nombre = "Efectivo" };
        var categoria = new Categoria { Nombre = "Periféricos" };
        var marca = new Marca { Nombre = "Logitech" };

        Context.Clientes.Add(cliente);
        Context.Empleados.Add(empleado);
        Context.MetodoPagos.Add(metodoPago);
        Context.Categorias.Add(categoria);
        Context.Marcas.Add(marca);
        await Context.SaveChangesAsync();

        var venta = new Venta
        {
            ClienteId = cliente.ClienteId,
            EmpleadoId = empleado.EmpleadoId,
            MetodoPagoId = metodoPago.MetodoPagoId,
            Fecha = new DateTime(2026, 11, 27, 11, 0, 0),
            Subtotal = 2000m,
            Itbis = 360m,
            Total = 2360m
        };
        var producto = new Producto
        {
            CategoriaId = categoria.CategoriaId,
            MarcaId = marca.MarcaId,
            Nombre = "Teclado",
            Costo = 600m,
            Precio = 1000m
        };

        Context.Ventas.Add(venta);
        Context.Productos.Add(producto);
        await Context.SaveChangesAsync();

        return (venta.VentaId, producto.ProductoId);
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
        var (ventaId, productoId) = await SembrarDependenciasAsync();

        Context.VentaDetalles.AddRange(
            new VentaDetalle
            {
                VentaId = ventaId,
                ProductoId = productoId,
                Cantidad = 2,
                Precio = 1000m,
                Costo = 600m,
                Itbis = 360m
            },
            new VentaDetalle
            {
                VentaId = ventaId,
                ProductoId = productoId,
                Cantidad = 1,
                Precio = 1000m,
                Costo = 600m,
                Itbis = 180m
            }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodos();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, d => d.Cantidad == 2 && d.Itbis == 360m);
        Assert.Contains(resultado, d => d.Cantidad == 1 && d.Itbis == 180m);
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaVentaDetalleDto()
    {
        var (ventaId, productoId) = await SembrarDependenciasAsync();
        var entidad = new VentaDetalle
        {
            VentaId = ventaId,
            ProductoId = productoId,
            Cantidad = 3,
            Precio = 1500m,
            Costo = 900m,
            Itbis = 810m
        };
        Context.VentaDetalles.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.VentaDetalleId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.VentaDetalleId, resultado!.VentaDetalleId);
        Assert.Equal(ventaId, resultado.VentaId);
        Assert.Equal(productoId, resultado.ProductoId);
        Assert.Equal(3, resultado.Cantidad);
        Assert.Equal(1500m, resultado.Precio);
        Assert.Equal(900m, resultado.Costo);
        Assert.Equal(810m, resultado.Itbis);
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
        var (ventaId, productoId) = await SembrarDependenciasAsync();
        var dto = new VentaDetalleDto
        {
            VentaId = ventaId,
            ProductoId = productoId,
            Cantidad = 2,
            Precio = 1200m,
            Costo = 700m,
            Itbis = 432m
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.VentaDetalleId > 0);
        Assert.Equal(1200m, resultado.Precio);
        Assert.Equal(1, await Context.VentaDetalles.CountAsync());
    }

    [Fact]
    public async Task Crear_VentaOProductoInexistente_LanzaDbUpdateException()
    {
        var dto = new VentaDetalleDto
        {
            VentaId = 9999,
            ProductoId = 9999,
            Cantidad = 1,
            Precio = 100m,
            Costo = 50m,
            Itbis = 18m
        };

        await Assert.ThrowsAsync<DbUpdateException>(() => _service.Crear(dto));
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaCamposYRetornaTrue()
    {
        var (venta1, producto1) = await SembrarDependenciasAsync();
        var (venta2, producto2) = await SembrarDependenciasAsync();

        var entidad = new VentaDetalle
        {
            VentaId = venta1,
            ProductoId = producto1,
            Cantidad = 1,
            Precio = 1000m,
            Costo = 600m,
            Itbis = 180m
        };
        Context.VentaDetalles.Add(entidad);
        await Context.SaveChangesAsync();

        var dtoActualizado = new VentaDetalleDto
        {
            VentaDetalleId = entidad.VentaDetalleId,
            VentaId = venta2,
            ProductoId = producto2,
            Cantidad = 4,
            Precio = 1250m,
            Costo = 800m,
            Itbis = 900m
        };

        var actualizado = await _service.Actualizar(entidad.VentaDetalleId, dtoActualizado);
        var enDb = await Context.VentaDetalles.FindAsync(entidad.VentaDetalleId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal(venta2, enDb!.VentaId);
        Assert.Equal(producto2, enDb.ProductoId);
        Assert.Equal(4, enDb.Cantidad);
        Assert.Equal(1250m, enDb.Precio);
        Assert.Equal(800m, enDb.Costo);
        Assert.Equal(900m, enDb.Itbis);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new VentaDetalleDto { Cantidad = 1, Precio = 10m, Costo = 5m, Itbis = 1.8m };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var (ventaId, productoId) = await SembrarDependenciasAsync();
        var entidad = new VentaDetalle
        {
            VentaId = ventaId,
            ProductoId = productoId,
            Cantidad = 1,
            Precio = 500m,
            Costo = 300m,
            Itbis = 90m
        };
        Context.VentaDetalles.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.VentaDetalleId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.VentaDetalles.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
