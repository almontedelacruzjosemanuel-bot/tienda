using Microsoft.EntityFrameworkCore;
using BonaoSells.Application.DTOs;
using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;

namespace BonaoSells.Application.Services;

public class CompraDetalleService
{
    private readonly BonaoSellsContext _context;

    public CompraDetalleService(BonaoSellsContext context)
    {
        _context = context;
    }

    public async Task<List<CompraDetalleDto>> ObtenerTodos()
    {
        return await _context.CompraDetalles
            .Select(d => new CompraDetalleDto
            {
                CompraDetalleId = d.CompraDetalleId,
                CompraId = d.CompraId,
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                CostoUnitario = d.CostoUnitario,
                Subtotal = d.Subtotal
            })
            .ToListAsync();
    }

    public async Task<CompraDetalleDto?> ObtenerPorId(int id)
    {
        var detalle = await _context.CompraDetalles.FindAsync(id);

        if (detalle == null)
            return null;

        return new CompraDetalleDto
        {
            CompraDetalleId = detalle.CompraDetalleId,
            CompraId = detalle.CompraId,
            ProductoId = detalle.ProductoId,
            Cantidad = detalle.Cantidad,
            CostoUnitario = detalle.CostoUnitario,
            Subtotal = detalle.Subtotal
        };
    }

    public async Task<CompraDetalleDto> Crear(CompraDetalleDto dto)
    {
        var detalle = new CompraDetalle
        {
            CompraId = dto.CompraId,
            ProductoId = dto.ProductoId,
            Cantidad = dto.Cantidad,
            CostoUnitario = dto.CostoUnitario,
            Subtotal = dto.Subtotal
        };

        _context.CompraDetalles.Add(detalle);
        await _context.SaveChangesAsync();

        dto.CompraDetalleId = detalle.CompraDetalleId;
        return dto;
    }

    public async Task<bool> Actualizar(int id, CompraDetalleDto dto)
    {
        var detalle = await _context.CompraDetalles.FindAsync(id);

        if (detalle == null)
            return false;

        detalle.CompraId = dto.CompraId;
        detalle.ProductoId = dto.ProductoId;
        detalle.Cantidad = dto.Cantidad;
        detalle.CostoUnitario = dto.CostoUnitario;
        detalle.Subtotal = dto.Subtotal;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var detalle = await _context.CompraDetalles.FindAsync(id);

        if (detalle == null)
            return false;

        _context.CompraDetalles.Remove(detalle);
        await _context.SaveChangesAsync();
        return true;
    }
}
