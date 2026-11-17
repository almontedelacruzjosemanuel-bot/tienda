
using Microsoft.EntityFrameworkCore;
using tienda.DTOs;
using tienda.Models;

namespace tienda.Services;

public class ClienteService
{
    private readonly TiendaContext _context;

    public ClienteService(TiendaContext context)
    {
        _context = context;
    }

    public async Task<List<ClienteDto>> ObtenerTodos()
    {
        return await _context.Clientes
            .Select(c => new ClienteDto
            {
                IdCliente = c.IdCliente,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                DocumentoIdentidad = c.DocumentoIdentidad,
                Email = c.Email,
                Telefono = c.Telefono,
                Direccion = c.Direccion,
                FechaRegistro = c.FechaRegistro
            })
            .ToListAsync();
    }

    public async Task<ClienteDto?> ObtenerPorId(int id)
    {
        return await _context.Clientes
            .Where(c => c.IdCliente == id)
            .Select(c => new ClienteDto
            {
                IdCliente = c.IdCliente,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                DocumentoIdentidad = c.DocumentoIdentidad,
                Email = c.Email,
                Telefono = c.Telefono,
                Direccion = c.Direccion,
                FechaRegistro = c.FechaRegistro
            })
            .FirstOrDefaultAsync();
    }
}
