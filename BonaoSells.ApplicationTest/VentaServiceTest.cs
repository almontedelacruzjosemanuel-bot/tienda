using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using BonaoSells.ApplicationTest.Helpers;

namespace BonaoSells.ApplicationTest.Services;

public class VentaServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly BonaoSellsContext _context;
    private readonly VentaService _service;

    public VentaServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        using (var pragmaCmd = _connection.CreateCommand())
        {
            pragmaCmd.CommandText = "PRAGMA foreign_keys = ON;";
            pragmaCmd.ExecuteNonQuery();
        }

        var options = new DbContextOptionsBuilder<BonaoSellsContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new TestBonaoSellsContext(options);
        _context.Database.EnsureCreated();

        _service = new VentaService(_context);
    }

    private async Task<(int ClienteId, int EmpleadoId, int MetodoPagoId)> SembrarCatalogosBasicosAsync()
    {
        var fechaHoy = DateOnly.FromDateTime(DateTime.Today);

        var cliente = new Cliente
        {
            NombreCompleto = "Juan Pérez",
            DocumentoIdentidad = "001-0000000-1",
            FechaRegistro = fechaHoy
        };
        var empleado = new Empleado
        {
            Nombre = "Ana",
            Apellido = "Gómez",
            Cargo = "Cajera",
            FechaContratacion = fechaHoy
        };
        var metodoPago = new MetodoPago
        {
            Nombre = "Efectivo"
        };

        _context.Clientes.Add(cliente);
        _context.Empleados.Add(empleado);
        _context.MetodoPagos.Add(metodoPago);
        await _context.SaveChangesAsync();

        return (cliente.ClienteId, empleado.EmpleadoId, metodoPago.MetodoPagoId);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoNoHayVentas_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodas();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoExistenVentas_RetornaTodasMapeadas()
    {
        var (clienteId, empleadoId, metodoPagoId) = await SembrarCatalogosBasicosAsync();
        var fecha = new DateTime(2026, 11, 26, 10, 30, 0);

        _context.Ventas.AddRange(
            new Venta
            {
                ClienteId = clienteId,
                EmpleadoId = empleadoId,
                MetodoPagoId = metodoPagoId,
                Fecha = fecha,
                Subtotal = 1000m,
                Itbis = 180m,
                Total = 1180m,
                ComprobanteFiscal = "B0100000001"
            },
            new Venta
            {
                ClienteId = clienteId,
                EmpleadoId = empleadoId,
                MetodoPagoId = metodoPagoId,
                Fecha = fecha.AddHours(1),
                Subtotal = 500m,
                Itbis = 90m,
                Total = 590m,
                ComprobanteFiscal = "B0200000001"
            }
        );
        await _context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodas();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, v => v.ComprobanteFiscal == "B0100000001" && v.Total == 1180m);
        Assert.Contains(resultado, v => v.ComprobanteFiscal == "B0200000001" && v.Total == 590m);
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaVentaDtoConMontosYComprobante()
    {
        var (clienteId, empleadoId, metodoPagoId) = await SembrarCatalogosBasicosAsync();
        var fecha = new DateTime(2026, 11, 26, 11, 0, 0);

        var venta = new Venta
        {
            ClienteId = clienteId,
            EmpleadoId = empleadoId,
            MetodoPagoId = metodoPagoId,
            Fecha = fecha,
            Subtotal = 2000m,
            Itbis = 360m,
            Total = 2360m,
            ComprobanteFiscal = "B0100000002"
        };
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(venta.VentaId);

        Assert.NotNull(resultado);
        Assert.Equal(venta.VentaId, resultado!.VentaId);
        Assert.Equal(clienteId, resultado.ClienteId);
        Assert.Equal(empleadoId, resultado.EmpleadoId);
        Assert.Equal(metodoPagoId, resultado.MetodoPagoId);
        Assert.Equal(fecha, resultado.Fecha);
        Assert.Equal(2000m, resultado.Subtotal);
        Assert.Equal(360m, resultado.Itbis);
        Assert.Equal(2360m, resultado.Total);
        Assert.Equal("B0100000002", resultado.ComprobanteFiscal);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(9999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_VentaValida_GuardaEnBaseYAsignaVentaId()
    {
        var (clienteId, empleadoId, metodoPagoId) = await SembrarCatalogosBasicosAsync();
        var dto = new VentaDto
        {
            ClienteId = clienteId,
            EmpleadoId = empleadoId,
            MetodoPagoId = metodoPagoId,
            Fecha = new DateTime(2026, 11, 26, 11, 15, 0),
            Subtotal = 1500m,
            Itbis = 270m,
            Total = 1770m,
            ComprobanteFiscal = "B0100000003"
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.VentaId > 0);
        Assert.Equal(1770m, resultado.Total);
        Assert.Equal(1, await _context.Ventas.CountAsync());
    }

    [Fact]
    public async Task Crear_ClienteInexistenteConForeignKeysActivas_LanzaDbUpdateException()
    {
        var (_, empleadoId, metodoPagoId) = await SembrarCatalogosBasicosAsync();
        var dto = new VentaDto
        {
            ClienteId = 9999,
            EmpleadoId = empleadoId,
            MetodoPagoId = metodoPagoId,
            Fecha = DateTime.UtcNow,
            Subtotal = 100m,
            Itbis = 18m,
            Total = 118m
        };

        await Assert.ThrowsAsync<DbUpdateException>(() => _service.Crear(dto));
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaCamposYRetornaTrue()
    {
        var (clienteId, empleadoId, metodoPagoId) = await SembrarCatalogosBasicosAsync();
        var venta = new Venta
        {
            ClienteId = clienteId,
            EmpleadoId = empleadoId,
            MetodoPagoId = metodoPagoId,
            Fecha = new DateTime(2026, 11, 26, 9, 0, 0),
            Subtotal = 500m,
            Itbis = 90m,
            Total = 590m,
            ComprobanteFiscal = "B0200000010"
        };
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();

        var dtoActualizado = new VentaDto
        {
            VentaId = venta.VentaId,
            ClienteId = clienteId,
            EmpleadoId = empleadoId,
            MetodoPagoId = metodoPagoId,
            Fecha = new DateTime(2026, 11, 26, 10, 0, 0),
            Subtotal = 800m,
            Itbis = 144m,
            Total = 944m,
            ComprobanteFiscal = "B0100000010"
        };

        var actualizado = await _service.Actualizar(venta.VentaId, dtoActualizado);
        var enDb = await _context.Ventas.FindAsync(venta.VentaId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal(800m, enDb!.Subtotal);
        Assert.Equal(144m, enDb.Itbis);
        Assert.Equal(944m, enDb.Total);
        Assert.Equal("B0100000010", enDb.ComprobanteFiscal);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new VentaDto
        {
            Fecha = DateTime.UtcNow,
            Subtotal = 100m,
            Itbis = 18m,
            Total = 118m
        };

        var actualizado = await _service.Actualizar(9999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaVentaYRetornaTrue()
    {
        var (clienteId, empleadoId, metodoPagoId) = await SembrarCatalogosBasicosAsync();
        var venta = new Venta
        {
            ClienteId = clienteId,
            EmpleadoId = empleadoId,
            MetodoPagoId = metodoPagoId,
            Fecha = DateTime.UtcNow,
            Subtotal = 300m,
            Itbis = 54m,
            Total = 354m
        };
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(venta.VentaId);

        Assert.True(eliminado);
        Assert.Equal(0, await _context.Ventas.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(9999);

        Assert.False(eliminado);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}