using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BonaoSells.Application.DTOs;
using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;

namespace BonaoSells.Application.Services;

public class UsuarioService
{
    private readonly BonaoSellsContext _context;
    private readonly PasswordHasher<Usuario> _passwordHasher;

    public UsuarioService(BonaoSellsContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<Usuario>();
    }

    public async Task<List<UsuarioDto>> ObtenerTodos()
    {
        return await _context.Usuarios
            .Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                EmpleadoId = u.EmpleadoId,
                NombreUsuario = u.NombreUsuario,
                Activo = u.Activo
            })
            .ToListAsync();
    }

    public async Task<UsuarioDto?> ObtenerPorId(int id)
    {
        return await _context.Usuarios
            .Where(u => u.UsuarioId == id)
            .Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                EmpleadoId = u.EmpleadoId,
                NombreUsuario = u.NombreUsuario,
                Activo = u.Activo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UsuarioDto> Crear(UsuarioDto dto, string contrasena)
    {
        var usuario = new Usuario
        {
            EmpleadoId = dto.EmpleadoId,
            NombreUsuario = dto.NombreUsuario,
            Activo = dto.Activo
        };

        usuario.ContrasenaHash = _passwordHasher.HashPassword(usuario, contrasena);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        dto.UsuarioId = usuario.UsuarioId;

        return dto;
    }

    public async Task<bool> Actualizar(int id, UsuarioDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
            return false;

        usuario.EmpleadoId = dto.EmpleadoId;
        usuario.NombreUsuario = dto.NombreUsuario;
        usuario.Activo = dto.Activo;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CambiarContrasena(int id, string nuevaContrasena)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
            return false;

        usuario.ContrasenaHash = _passwordHasher.HashPassword(usuario, nuevaContrasena);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
            return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return true;
    }
}
