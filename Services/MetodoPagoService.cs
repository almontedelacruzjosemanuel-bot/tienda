
using Microsoft.EntityFrameworkCore;
using tienda.DTOs;
using tienda.Models;

namespace tienda.Services;

public class MetodoPagoService
{
    private readonly TiendaContext _context;

    public MetodoPagoService(TiendaContext context)
    {
        _context = context;
    }

    public async Task<List<MetodoPagoDto>> ObtenerTodos()
    {
        return await _context.MetodoPagos
            .Select(m => new MetodoPagoDto
            {
                MetodoPagoId = m.MetodoPagoId,
                Nombre = m.Nombre
            })
            .ToListAsync();
    }

    public async Task<MetodoPagoDto?> ObtenerPorId(int id)
    {
        return await _context.MetodoPagos
            .Where(m => m.MetodoPagoId == id)
            .Select(m => new MetodoPagoDto
            {
                MetodoPagoId = m.MetodoPagoId,
                Nombre = m.Nombre
            })
            .FirstOrDefaultAsync();
    }

    public async Task<MetodoPagoDto> Crear(MetodoPagoDto dto)
    {
        var metodoPago = new MetodoPago
        {
            Nombre = dto.Nombre
        };

        _context.MetodoPagos.Add(metodoPago);
        await _context.SaveChangesAsync();

        dto.MetodoPagoId = metodoPago.MetodoPagoId;

        return dto;
    }

    public async Task<bool> Actualizar(int id, MetodoPagoDto dto)
    {
        var metodoPago = await _context.MetodoPagos.FindAsync(id);

        if (metodoPago == null)
            return false;

        metodoPago.Nombre = dto.Nombre;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var metodoPago = await _context.MetodoPagos.FindAsync(id);

        if (metodoPago == null)
            return false;

        _context.MetodoPagos.Remove(metodoPago);
        await _context.SaveChangesAsync();

        return true;
    }
}
