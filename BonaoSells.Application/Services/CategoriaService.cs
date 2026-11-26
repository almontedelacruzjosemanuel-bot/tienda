using Microsoft.EntityFrameworkCore;
using BonaoSells.Application.DTOs;
using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;

namespace BonaoSells.Application.Services;

public class CategoriaService
{
    private readonly BonaoSellsContext _context;

    public CategoriaService(BonaoSellsContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaDto>> ObtenerTodas()
    {
        return await _context.Categorias
            .Select(c => new CategoriaDto
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            })
            .ToListAsync();
    }

    public async Task<CategoriaDto?> ObtenerPorId(int id)
    {
        return await _context.Categorias
            .Where(c => c.CategoriaId == id)
            .Select(c => new CategoriaDto
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoriaDto> Crear(CategoriaDto dto)
    {
        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        dto.CategoriaId = categoria.CategoriaId;

        return dto;
    }

    public async Task<bool> Actualizar(int id, CategoriaDto dto)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null)
            return false;

        categoria.Nombre = dto.Nombre;
        categoria.Descripcion = dto.Descripcion;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null)
            return false;

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return true;
    }
}
