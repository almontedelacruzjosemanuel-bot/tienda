
using Microsoft.EntityFrameworkCore;
using tienda.DTOs;
using tienda.Models;

namespace tienda.Services;

public class MarcaService
{
    private readonly TiendaContext _context;

    public MarcaService(TiendaContext context)
    {
        _context = context;
    }

    public async Task<List<MarcaDto>> ObtenerTodas()
    {
        return await _context.Marcas
            .Select(m => new MarcaDto
            {
                IdMarca = m.IdMarca,
                Nombre = m.Nombre,
                PaisOrigen = m.PaisOrigen,
                SitioWeb = m.SitioWeb
            })
            .ToListAsync();
    }

    public async Task<MarcaDto?> ObtenerPorId(int id)
    {
        return await _context.Marcas
            .Where(m => m.IdMarca == id)
            .Select(m => new MarcaDto
            {
                IdMarca = m.IdMarca,
                Nombre = m.Nombre,
                PaisOrigen = m.PaisOrigen,
                SitioWeb = m.SitioWeb
            })
            .FirstOrDefaultAsync();
    }
}
