
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
        return await _context.Proveedores
            .Select(p => new ProveedorDto
            {
                ProveedorId = p.ProveedorId,
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
        return await _context.Proveedores
            .Where(p => p.ProveedorId == id)
            .Select(p => new ProveedorDto
            {
                ProveedorId = p.ProveedorId,
                NombreEmpresa = p.NombreEmpresa,
                ContactoNombre = p.ContactoNombre,
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProveedorDto> Crear(ProveedorDto dto)
    {
        var proveedor = new Proveedor
        {
            NombreEmpresa = dto.NombreEmpresa,
            ContactoNombre = dto.ContactoNombre,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Direccion = dto.Direccion
        };

        _context.Proveedores.Add(proveedor);
        await _context.SaveChangesAsync();

        dto.ProveedorId = proveedor.ProveedorId;

        return dto;
    }

    public async Task<bool> Actualizar(int id, ProveedorDto dto)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);

        if (proveedor == null)
            return false;

        proveedor.NombreEmpresa = dto.NombreEmpresa;
        proveedor.ContactoNombre = dto.ContactoNombre;
        proveedor.Telefono = dto.Telefono;
        proveedor.Email = dto.Email;
        proveedor.Direccion = dto.Direccion;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);

        if (proveedor == null)
            return false;

        _context.Proveedores.Remove(proveedor);
        await _context.SaveChangesAsync();

        return true;
    }
}
