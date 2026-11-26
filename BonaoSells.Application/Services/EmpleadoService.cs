using Microsoft.EntityFrameworkCore;
using BonaoSells.Application.DTOs;
using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;

namespace BonaoSells.Application.Services;

public class EmpleadoService
{
    private readonly BonaoSellsContext _context;

    public EmpleadoService(BonaoSellsContext context)
    {
        _context = context;
    }

    public async Task<List<EmpleadoDto>> ObtenerTodos()
    {
        return await _context.Empleados
            .Select(e => new EmpleadoDto
            {
                EmpleadoId = e.EmpleadoId,
                Nombre = e.Nombre,
                Apellido = e.Apellido,
                Cargo = e.Cargo,
                Email = e.Email,
                Telefono = e.Telefono,
                FechaContratacion = e.FechaContratacion
            })
            .ToListAsync();
    }

    public async Task<EmpleadoDto?> ObtenerPorId(int id)
    {
        return await _context.Empleados
            .Where(e => e.EmpleadoId == id)
            .Select(e => new EmpleadoDto
            {
                EmpleadoId = e.EmpleadoId,
                Nombre = e.Nombre,
                Apellido = e.Apellido,
                Cargo = e.Cargo,
                Email = e.Email,
                Telefono = e.Telefono,
                FechaContratacion = e.FechaContratacion
            })
            .FirstOrDefaultAsync();
    }

    public async Task<EmpleadoDto> Crear(EmpleadoDto dto)
    {
        var empleado = new Empleado
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Cargo = dto.Cargo,
            Email = dto.Email,
            Telefono = dto.Telefono,
            FechaContratacion = dto.FechaContratacion
        };

        _context.Empleados.Add(empleado);
        await _context.SaveChangesAsync();

        dto.EmpleadoId = empleado.EmpleadoId;

        return dto;
    }

    public async Task<bool> Actualizar(int id, EmpleadoDto dto)
    {
        var empleado = await _context.Empleados.FindAsync(id);

        if (empleado == null)
            return false;

        empleado.Nombre = dto.Nombre;
        empleado.Apellido = dto.Apellido;
        empleado.Cargo = dto.Cargo;
        empleado.Email = dto.Email;
        empleado.Telefono = dto.Telefono;
        empleado.FechaContratacion = dto.FechaContratacion;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);

        if (empleado == null)
            return false;

        _context.Empleados.Remove(empleado);
        await _context.SaveChangesAsync();

        return true;
    }
}
