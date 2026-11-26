using Microsoft.EntityFrameworkCore;
using BonaoSells.Application.DTOs;
using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;

namespace BonaoSells.Application.Services;


public class CiudadService
{
    private readonly BonaoSellsContext _context;

    public CiudadService(BonaoSellsContext context)
    {
        _context = context;
    }

    public async Task<List<CiudadDto>> ObtenerTodas()
    {
        return await _context.Ciudades
            .Select(c => new CiudadDto
            {
                CiudadId = c.CiudadId,
                Nombre = c.Nombre
            })
            .ToListAsync();
    }

    public async Task<CiudadDto?> ObtenerPorId(int id)
    {
        return await _context.Ciudades
            .Where(c => c.CiudadId == id)
            .Select(c => new CiudadDto
            {
                CiudadId = c.CiudadId,
                Nombre = c.Nombre
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CiudadDto> Crear(CiudadDto dto)
    {
        var ciudad = new Ciudad
        {
            Nombre = dto.Nombre
        };

        _context.Ciudades.Add(ciudad);
        await _context.SaveChangesAsync();

        dto.CiudadId = ciudad.CiudadId;

        return dto;
    }

    public async Task<bool> Actualizar(int id, CiudadDto dto)
    {
        var ciudad = await _context.Ciudades.FindAsync(id);

        if (ciudad == null)
            return false;

        ciudad.Nombre = dto.Nombre;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var ciudad = await _context.Ciudades.FindAsync(id);

        if (ciudad == null)
            return false;

        _context.Ciudades.Remove(ciudad);
        await _context.SaveChangesAsync();

        return true;
    }
}