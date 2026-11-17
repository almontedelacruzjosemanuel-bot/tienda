
using Microsoft.EntityFrameworkCore;
using tienda.DTOs;
using tienda.Models;

namespace tienda.Services;

public class CategoriaService
{
    private readonly TiendaContext _context;

    public CategoriaService(TiendaContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaDto>> ObtenerTodas()
    {
        return await _context.Categorias
            .Select(c => new CategoriaDto
            {
                IdCategoria = c.IdCategoria,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            })
            .ToListAsync();
    }

    public async Task<CategoriaDto?> ObtenerPorId(int id)
    {
        return await _context.Categorias
            .Where(c => c.IdCategoria == id)
            .Select(c => new CategoriaDto
            {
                IdCategoria = c.IdCategoria,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            })
            .FirstOrDefaultAsync();
    }
}
