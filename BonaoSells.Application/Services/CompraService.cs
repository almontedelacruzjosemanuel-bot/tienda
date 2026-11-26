using Microsoft.EntityFrameworkCore;
using BonaoSells.Application.DTOs;
using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;

namespace BonaoSells.Application.Services;

public class CompraService
{
    private readonly BonaoSellsContext _context;

    public CompraService(BonaoSellsContext context)
    {
        _context = context;
    }

    public async Task<List<CompraDto>> ObtenerTodas()
    {
        return await _context.Compras
            .Select(c => new CompraDto
            {
                CompraId = c.CompraId,
                ProveedorId = c.ProveedorId,
                EmpleadoId = c.EmpleadoId,
                Fecha = c.Fecha,
                Total = c.Total,
                Estado = c.Estado
            })
            .ToListAsync();
    }

    public async Task<CompraDto?> ObtenerPorId(int id)
    {
        var compra = await _context.Compras.FindAsync(id);

        if (compra == null)
            return null;

        return new CompraDto
        {
            CompraId = compra.CompraId,
            ProveedorId = compra.ProveedorId,
            EmpleadoId = compra.EmpleadoId,
            Fecha = compra.Fecha,
            Total = compra.Total,
            Estado = compra.Estado
        };
    }

    public async Task<CompraDto> Crear(CompraDto dto)
    {
        var compra = new Compra
        {
            ProveedorId = dto.ProveedorId,
            EmpleadoId = dto.EmpleadoId,
            Fecha = dto.Fecha,
            Total = dto.Total,
            Estado = dto.Estado
        };

        _context.Compras.Add(compra);
        await _context.SaveChangesAsync();

        dto.CompraId = compra.CompraId;
        return dto;
    }

    public async Task<bool> Actualizar(int id, CompraDto dto)
    {
        var compra = await _context.Compras.FindAsync(id);

        if (compra == null)
            return false;

        compra.ProveedorId = dto.ProveedorId;
        compra.EmpleadoId = dto.EmpleadoId;
        compra.Fecha = dto.Fecha;
        compra.Total = dto.Total;
        compra.Estado = dto.Estado;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var compra = await _context.Compras.FindAsync(id);

        if (compra == null)
            return false;

        _context.Compras.Remove(compra);
        await _context.SaveChangesAsync();
        return true;
    }
}
