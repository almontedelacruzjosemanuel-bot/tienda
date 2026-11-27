using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class UsuarioServiceTests : SqliteServiceTestBase
{
    private readonly UsuarioService _service;
    private readonly PasswordHasher<Usuario> _hasher = new();

    public UsuarioServiceTests()
    {
        _service = new UsuarioService(Context);
    }

    private async Task<int> SembrarEmpleadoAsync(string nombre = "Ana", string apellido = "Gómez")
    {
        var empleado = new Empleado
        {
            Nombre = nombre,
            Apellido = apellido,
            Cargo = "Vendedor",
            FechaContratacion = new DateOnly(2026, 1, 10)
        };
        Context.Empleados.Add(empleado);
        await Context.SaveChangesAsync();
        return empleado.EmpleadoId;
    }

    [Fact]
    public async Task ObtenerTodos_CuandoNoHayUsuarios_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodos();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoExistenUsuarios_RetornaTodosMapeados()
    {
        var empleadoId = await SembrarEmpleadoAsync();

        Context.Usuarios.AddRange(
            new Usuario
            {
                EmpleadoId = empleadoId,
                NombreUsuario = "agomez",
                ContrasenaHash = "hash1",
                Activo = true
            },
            new Usuario
            {
                EmpleadoId = empleadoId,
                NombreUsuario = "agomez_inactivo",
                ContrasenaHash = "hash2",
                Activo = false
            }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodos();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, u => u.NombreUsuario == "agomez" && u.Activo);
        Assert.Contains(resultado, u => u.NombreUsuario == "agomez_inactivo" && !u.Activo);
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaUsuarioDto()
    {
        var empleadoId = await SembrarEmpleadoAsync();
        var entidad = new Usuario
        {
            EmpleadoId = empleadoId,
            NombreUsuario = "admin",
            ContrasenaHash = "hash_seguro",
            Activo = true
        };
        Context.Usuarios.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.UsuarioId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.UsuarioId, resultado!.UsuarioId);
        Assert.Equal(empleadoId, resultado.EmpleadoId);
        Assert.Equal("admin", resultado.NombreUsuario);
        Assert.True(resultado.Activo);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_UsuarioValido_GuardaEnBaseHasheaContrasenaYAsignaId()
    {
        var empleadoId = await SembrarEmpleadoAsync();
        var dto = new UsuarioDto
        {
            EmpleadoId = empleadoId,
            NombreUsuario = "jperez",
            Activo = true
        };

        var resultado = await _service.Crear(dto, "ClaveSegura123!");
        var enDb = await Context.Usuarios.FindAsync(resultado.UsuarioId);

        Assert.True(resultado.UsuarioId > 0);
        Assert.Equal("jperez", resultado.NombreUsuario);
        Assert.NotNull(enDb);
        Assert.NotEqual("ClaveSegura123!", enDb!.ContrasenaHash);
        Assert.Equal(
            PasswordVerificationResult.Success,
            _hasher.VerifyHashedPassword(enDb, enDb.ContrasenaHash, "ClaveSegura123!")
        );
    }

    [Fact]
    public async Task Crear_EmpleadoInexistente_LanzaDbUpdateException()
    {
        var dto = new UsuarioDto
        {
            EmpleadoId = 9999,
            NombreUsuario = "invalido",
            Activo = true
        };

        await Assert.ThrowsAsync<DbUpdateException>(() => _service.Crear(dto, "Pass123!"));
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaCamposYPreservaContrasenaHash()
    {
        var empleado1 = await SembrarEmpleadoAsync("Ana", "Gómez");
        var empleado2 = await SembrarEmpleadoAsync("Luis", "Díaz");

        var creado = await _service.Crear(new UsuarioDto
        {
            EmpleadoId = empleado1,
            NombreUsuario = "usuario_viejo",
            Activo = true
        }, "ClaveInicial123!");

        var dtoActualizado = new UsuarioDto
        {
            UsuarioId = creado.UsuarioId,
            EmpleadoId = empleado2,
            NombreUsuario = "usuario_nuevo",
            Activo = false
        };

        var actualizado = await _service.Actualizar(creado.UsuarioId, dtoActualizado);
        var enDb = await Context.Usuarios.FindAsync(creado.UsuarioId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal(empleado2, enDb!.EmpleadoId);
        Assert.Equal("usuario_nuevo", enDb.NombreUsuario);
        Assert.False(enDb.Activo);
        Assert.Equal(
            PasswordVerificationResult.Success,
            _hasher.VerifyHashedPassword(enDb, enDb.ContrasenaHash, "ClaveInicial123!")
        );
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new UsuarioDto
        {
            EmpleadoId = 1,
            NombreUsuario = "no_existe",
            Activo = true
        };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task CambiarContrasena_IdExistente_ActualizaHashConNuevaContrasenaYRetornaTrue()
    {
        var empleadoId = await SembrarEmpleadoAsync();
        var creado = await _service.Crear(new UsuarioDto
        {
            EmpleadoId = empleadoId,
            NombreUsuario = "mperez",
            Activo = true
        }, "ClaveVieja1!");

        var cambiado = await _service.CambiarContrasena(creado.UsuarioId, "ClaveNueva2!");
        var enDb = await Context.Usuarios.FindAsync(creado.UsuarioId);

        Assert.True(cambiado);
        Assert.NotNull(enDb);
        Assert.Equal(
            PasswordVerificationResult.Success,
            _hasher.VerifyHashedPassword(enDb!, enDb!.ContrasenaHash, "ClaveNueva2!")
        );
        Assert.Equal(
            PasswordVerificationResult.Failed,
            _hasher.VerifyHashedPassword(enDb, enDb.ContrasenaHash, "ClaveVieja1!")
        );
    }

    [Fact]
    public async Task CambiarContrasena_IdInexistente_RetornaFalse()
    {
        var cambiado = await _service.CambiarContrasena(999, "NuevaClave123!");

        Assert.False(cambiado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var empleadoId = await SembrarEmpleadoAsync();
        var entidad = new Usuario
        {
            EmpleadoId = empleadoId,
            NombreUsuario = "temporal",
            ContrasenaHash = "hash",
            Activo = true
        };
        Context.Usuarios.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.UsuarioId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.Usuarios.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
