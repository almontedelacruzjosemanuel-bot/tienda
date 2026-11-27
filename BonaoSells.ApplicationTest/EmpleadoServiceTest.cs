using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class EmpleadoServiceTests : SqliteServiceTestBase
{
    private readonly EmpleadoService _service;

    public EmpleadoServiceTests()
    {
        _service = new EmpleadoService(Context);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoNoHayEmpleados_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodos();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoExistenEmpleados_RetornaTodosMapeados()
    {
        var fecha = new DateOnly(2025, 1, 15);
        Context.Empleados.AddRange(
            new Empleado
            {
                Nombre = "Ana",
                Apellido = "Gómez",
                Cargo = "Cajera",
                Email = "ana@bonaosells.do",
                Telefono = "809-555-1111",
                FechaContratacion = fecha
            },
            new Empleado
            {
                Nombre = "Luis",
                Apellido = "Martínez",
                Cargo = "Vendedor",
                Email = "luis@bonaosells.do",
                Telefono = "809-555-2222",
                FechaContratacion = fecha.AddMonths(2)
            }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodos();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, e => e.Nombre == "Ana" && e.Apellido == "Gómez" && e.Cargo == "Cajera");
        Assert.Contains(resultado, e => e.Nombre == "Luis" && e.Email == "luis@bonaosells.do");
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaEmpleadoDto()
    {
        var fecha = new DateOnly(2026, 3, 1);
        var entidad = new Empleado
        {
            Nombre = "Sofía",
            Apellido = "Pérez",
            Cargo = "Gerente",
            Email = "sofia@bonaosells.do",
            Telefono = "809-555-3333",
            FechaContratacion = fecha
        };
        Context.Empleados.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.EmpleadoId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.EmpleadoId, resultado!.EmpleadoId);
        Assert.Equal("Sofía", resultado.Nombre);
        Assert.Equal("Pérez", resultado.Apellido);
        Assert.Equal("Gerente", resultado.Cargo);
        Assert.Equal("sofia@bonaosells.do", resultado.Email);
        Assert.Equal("809-555-3333", resultado.Telefono);
        Assert.Equal(fecha, resultado.FechaContratacion);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_EmpleadoValido_GuardaEnBaseYAsignaId()
    {
        var dto = new EmpleadoDto
        {
            Nombre = "Miguel",
            Apellido = "Santos",
            Cargo = "Almacén",
            Email = "miguel@bonaosells.do",
            Telefono = "829-555-4444",
            FechaContratacion = new DateOnly(2026, 11, 1)
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.EmpleadoId > 0);
        Assert.Equal("Miguel", resultado.Nombre);
        Assert.Equal(1, await Context.Empleados.CountAsync());
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaDatosYRetornaTrue()
    {
        var entidad = new Empleado
        {
            Nombre = "Carlos",
            Apellido = "Díaz",
            Cargo = "Auxiliar",
            Email = "carlos@bonaosells.do",
            Telefono = "809-111-1111",
            FechaContratacion = new DateOnly(2024, 5, 10)
        };
        Context.Empleados.Add(entidad);
        await Context.SaveChangesAsync();

        var nuevaFecha = new DateOnly(2025, 6, 15);
        var dtoActualizado = new EmpleadoDto
        {
            EmpleadoId = entidad.EmpleadoId,
            Nombre = "Carlos Manuel",
            Apellido = "Díaz Cruz",
            Cargo = "Supervisor",
            Email = "c.diaz@bonaosells.do",
            Telefono = "809-222-2222",
            FechaContratacion = nuevaFecha
        };

        var actualizado = await _service.Actualizar(entidad.EmpleadoId, dtoActualizado);
        var enDb = await Context.Empleados.FindAsync(entidad.EmpleadoId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal("Carlos Manuel", enDb!.Nombre);
        Assert.Equal("Díaz Cruz", enDb.Apellido);
        Assert.Equal("Supervisor", enDb.Cargo);
        Assert.Equal("c.diaz@bonaosells.do", enDb.Email);
        Assert.Equal("809-222-2222", enDb.Telefono);
        Assert.Equal(nuevaFecha, enDb.FechaContratacion);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new EmpleadoDto { Nombre = "Inexistente" };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var entidad = new Empleado { Nombre = "Temporal", Apellido = "Test" };
        Context.Empleados.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.EmpleadoId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.Empleados.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
