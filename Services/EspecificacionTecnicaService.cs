
using Microsoft.EntityFrameworkCore;
using tienda.DTOs;
using tienda.Models;

namespace tienda.Services;

public class EspecificacionTecnicaService
{
    private readonly TiendaContext _context;

    public EspecificacionTecnicaService(TiendaContext context)
    {
        _context = context;
    }

    public async Task<List<EspecificacionTecnicaDto>> ObtenerTodas()
    {
        return await _context.EspecificacionesTecnicas
            .Select(e => new EspecificacionTecnicaDto
            {
                EspecificacionId = e.EspecificacionId,
                ProductoId = e.ProductoId,
                Clave = e.Clave,
                Valor = e.Valor
            })
            .ToListAsync();
    }

    public async Task<EspecificacionTecnicaDto?> ObtenerPorId(int id)
    {
        return await _context.EspecificacionesTecnicas
            .Where(e => e.EspecificacionId == id)
            .Select(e => new EspecificacionTecnicaDto
            {
                EspecificacionId = e.EspecificacionId,
                ProductoId = e.ProductoId,
                Clave = e.Clave,
                Valor = e.Valor
            })
            .FirstOrDefaultAsync();
    }

    public async Task<EspecificacionTecnicaDto> Crear(EspecificacionTecnicaDto dto)
    {
        var especificacion = new EspecificacionTecnica
        {
            ProductoId = dto.ProductoId,
            Clave = dto.Clave,
            Valor = dto.Valor
        };

        _context.EspecificacionesTecnicas.Add(especificacion);
        await _context.SaveChangesAsync();

        dto.EspecificacionId = especificacion.EspecificacionId;

        return dto;
    }

    public async Task<bool> Actualizar(int id, EspecificacionTecnicaDto dto)
    {
        var especificacion = await _context.EspecificacionesTecnicas.FindAsync(id);

        if (especificacion == null)
            return false;

        especificacion.ProductoId = dto.ProductoId;
        especificacion.Clave = dto.Clave;
        especificacion.Valor = dto.Valor;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var especificacion = await _context.EspecificacionesTecnicas.FindAsync(id);

        if (especificacion == null)
            return false;

        _context.EspecificacionesTecnicas.Remove(especificacion);
        await _context.SaveChangesAsync();

        return true;
    }
}
