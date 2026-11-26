using Microsoft.EntityFrameworkCore;
using BonaoSells.Application.DTOs;
using BonaoSells.Infrastructure.Data;
using BonaoSells.Infrastructure.Models;

namespace BonaoSells.Application.Services;

public class VentaDetalleService
{
    private readonly BonaoSellsContext _context;

    public VentaDetalleService(BonaoSellsContext context)
    {
        _context = context;
    }

    public async Task<List<VentaDetalleDto>> ObtenerTodos()
    {
        return await _context.VentaDetalles
            .Select(d => new VentaDetalleDto
            {
                VentaDetalleId = d.VentaDetalleId,
                VentaId = d.VentaId,
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                Precio = d.Precio,
                Costo = d.Costo,
                Itbis = d.Itbis
            })
            .ToListAsync();
    }

    public async Task<VentaDetalleDto?> ObtenerPorId(int id)
    {
        var detalle = await _context.VentaDetalles.FindAsync(id);

        if (detalle == null)
            return null;

        return new VentaDetalleDto
        {
            VentaDetalleId = detalle.VentaDetalleId,
            VentaId = detalle.VentaId,
            ProductoId = detalle.ProductoId,
            Cantidad = detalle.Cantidad,
            Precio = detalle.Precio,
            Costo = detalle.Costo,
            Itbis = detalle.Itbis
        };
    }

    public async Task<VentaDetalleDto> Crear(VentaDetalleDto dto)
    {
        var detalle = new VentaDetalle
        {
            VentaId = dto.VentaId,
            ProductoId = dto.ProductoId,
            Cantidad = dto.Cantidad,
            Precio = dto.Precio,
            Costo = dto.Costo,
            Itbis = dto.Itbis
        };

        _context.VentaDetalles.Add(detalle);
        await _context.SaveChangesAsync();

        dto.VentaDetalleId = detalle.VentaDetalleId;
        return dto;
    }

    public async Task<bool> Actualizar(int id, VentaDetalleDto dto)
    {
        var detalle = await _context.VentaDetalles.FindAsync(id);

        if (detalle == null)
            return false;

        detalle.VentaId = dto.VentaId;
        detalle.ProductoId = dto.ProductoId;
        detalle.Cantidad = dto.Cantidad;
        detalle.Precio = dto.Precio;
        detalle.Costo = dto.Costo;
        detalle.Itbis = dto.Itbis;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var detalle = await _context.VentaDetalles.FindAsync(id);

        if (detalle == null)
            return false;

        _context.VentaDetalles.Remove(detalle);
        await _context.SaveChangesAsync();
        return true;
    }
}
