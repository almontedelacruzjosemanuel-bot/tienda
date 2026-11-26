using Microsoft.EntityFrameworkCore;
using BonaoSells.Application.DTOs;
using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;

namespace BonaoSells.Application.Services;

public class MarcaService
{
    private readonly BonaoSellsContext _context;

    public MarcaService(BonaoSellsContext context)
    {
        _context = context;
    }

    public async Task<List<MarcaDto>> ObtenerTodas()
    {
        return await _context.Marcas
            .Select(m => new MarcaDto
            {
                MarcaId = m.MarcaId,
                Nombre = m.Nombre,
                PaisOrigen = m.PaisOrigen,
                SitioWeb = m.SitioWeb
            })
            .ToListAsync();
    }

    public async Task<MarcaDto?> ObtenerPorId(int id)
    {
        return await _context.Marcas
            .Where(m => m.MarcaId == id)
            .Select(m => new MarcaDto
            {
                MarcaId = m.MarcaId,
                Nombre = m.Nombre,
                PaisOrigen = m.PaisOrigen,
                SitioWeb = m.SitioWeb
            })
            .FirstOrDefaultAsync();
    }

    public async Task<MarcaDto> Crear(MarcaDto dto)
    {
        var marca = new Marca
        {
            Nombre = dto.Nombre,
            PaisOrigen = dto.PaisOrigen,
            SitioWeb = dto.SitioWeb
        };

        _context.Marcas.Add(marca);
        await _context.SaveChangesAsync();

        dto.MarcaId = marca.MarcaId;

        return dto;
    }

    public async Task<bool> Actualizar(int id, MarcaDto dto)
    {
        var marca = await _context.Marcas.FindAsync(id);

        if (marca == null)
            return false;

        marca.Nombre = dto.Nombre;
        marca.PaisOrigen = dto.PaisOrigen;
        marca.SitioWeb = dto.SitioWeb;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var marca = await _context.Marcas.FindAsync(id);

        if (marca == null)
            return false;

        _context.Marcas.Remove(marca);
        await _context.SaveChangesAsync();

        return true;
    }
}
