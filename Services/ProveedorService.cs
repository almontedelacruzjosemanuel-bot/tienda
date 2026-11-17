
using Microsoft.EntityFrameworkCore;
using tienda.DTOs;
using tienda.Models;

namespace tienda.Services;

public class ProveedorService
{
    private readonly TiendaContext _context;

    public ProveedorService(TiendaContext context)
    {
        _context = context;
    }

    public async Task<List<ProveedorDto>> ObtenerTodos()
    {
        return await _context.Proveedors
            .Select(p => new ProveedorDto
            {
                IdProveedor = p.IdProveedor,
                NombreEmpresa = p.NombreEmpresa,
                ContactoNombre = p.ContactoNombre,
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion
            })
            .ToListAsync();
    }

    public async Task<ProveedorDto?> ObtenerPorId(int id)
    {
        return await _context.Proveedors
            .Where(p => p.IdProveedor == id)
            .Select(p => new ProveedorDto
            {
                IdProveedor = p.IdProveedor,
                NombreEmpresa = p.NombreEmpresa,
                ContactoNombre = p.ContactoNombre,
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion
            })
            .FirstOrDefaultAsync();
    }
}
