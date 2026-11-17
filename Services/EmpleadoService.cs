
using Microsoft.EntityFrameworkCore;
using tienda.DTOs;
using tienda.Models;

namespace tienda.Services;

public class EmpleadoService
{
    private readonly TiendaContext _context;

    public EmpleadoService(TiendaContext context)
    {
        _context = context;
    }

    public async Task<List<EmpleadoDto>> ObtenerTodos()
    {
        return await _context.Empleados
            .Select(e => new EmpleadoDto
            {
                IdEmpleado = e.IdEmpleado,
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
            .Where(e => e.IdEmpleado == id)
            .Select(e => new EmpleadoDto
            {
                IdEmpleado = e.IdEmpleado,
                Nombre = e.Nombre,
                Apellido = e.Apellido,
                Cargo = e.Cargo,
                Email = e.Email,
                Telefono = e.Telefono,
                FechaContratacion = e.FechaContratacion
            })
            .FirstOrDefaultAsync();
    }
}
