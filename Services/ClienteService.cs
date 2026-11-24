
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
                ClienteId = c.ClienteId,
                DocumentoIdentidad = c.DocumentoIdentidad,
                Email = c.Email,
                Telefono = c.Telefono,
                Direccion = c.Direccion,
                FechaRegistro = c.FechaRegistro,
                NombreCompleto = c.NombreCompleto,
                CiudadId = c.CiudadId
            })
            .ToListAsync();
    }

    public async Task<ClienteDto?> ObtenerPorId(int id)
    {
        return await _context.Clientes
            .Where(c => c.ClienteId == id)
            .Select(c => new ClienteDto
            {
                ClienteId = c.ClienteId,
                DocumentoIdentidad = c.DocumentoIdentidad,
                Email = c.Email,
                Telefono = c.Telefono,
                Direccion = c.Direccion,
                FechaRegistro = c.FechaRegistro,
                NombreCompleto = c.NombreCompleto,
                CiudadId = c.CiudadId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ClienteDto> Crear(ClienteDto dto)
    {
        var cliente = new Cliente
        {
            DocumentoIdentidad = dto.DocumentoIdentidad,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            FechaRegistro = dto.FechaRegistro,
            NombreCompleto = dto.NombreCompleto,
            CiudadId = dto.CiudadId
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        dto.ClienteId = cliente.ClienteId;

        return dto;
    }

    public async Task<bool> Actualizar(int id, ClienteDto dto)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return false;

        cliente.DocumentoIdentidad = dto.DocumentoIdentidad;
        cliente.Email = dto.Email;
        cliente.Telefono = dto.Telefono;
        cliente.Direccion = dto.Direccion;
        cliente.FechaRegistro = dto.FechaRegistro;
        cliente.NombreCompleto = dto.NombreCompleto;
        cliente.CiudadId = dto.CiudadId;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return false;

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return true;
    }
}
