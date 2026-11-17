
using Microsoft.EntityFrameworkCore;
using tienda.DTOs;
using tienda.Models;

namespace tienda.Services;

public class VentaService
{
    private readonly TiendaContext _context;

    public VentaService(TiendaContext context)
    {
        _context = context;
    }

    public async Task<List<VentaDto>> ObtenerTodas()
    {
        return await _context.Ventas
            .Select(v => new VentaDto
            {
                VentaId = v.VentaId,
                ClienteId = v.ClienteId,
                EmpleadoId = v.EmpleadoId,
                Fecha = v.Fecha,
                Total = v.Total,
                Subtotal = v.Subtotal,
                Itbis = v.Itbis,
                ComprobanteFiscal = v.ComprobanteFiscal,
                MetodoPagoId = v.MetodoPagoId
            })
            .ToListAsync();
    }

    public async Task<VentaDto?> ObtenerPorId(int id)
    {
        var venta = await _context.Ventas.FindAsync(id);

        if (venta == null)
            return null;

        return new VentaDto
        {
            VentaId = venta.VentaId,
            ClienteId = venta.ClienteId,
            EmpleadoId = venta.EmpleadoId,
            Fecha = venta.Fecha,
            Total = venta.Total,
            Subtotal = venta.Subtotal,
            Itbis = venta.Itbis,
            ComprobanteFiscal = venta.ComprobanteFiscal,
            MetodoPagoId = venta.MetodoPagoId
        };
    }

    public async Task<VentaDto> Crear(VentaDto dto)
    {
        var venta = new Venta
        {
            ClienteId = dto.ClienteId,
            EmpleadoId = dto.EmpleadoId,
            Fecha = dto.Fecha,
            Total = dto.Total,
            Subtotal = dto.Subtotal,
            Itbis = dto.Itbis,
            ComprobanteFiscal = dto.ComprobanteFiscal,
            MetodoPagoId = dto.MetodoPagoId
        };

        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();

        dto.VentaId = venta.VentaId;
        return dto;
    }

    public async Task<bool> Actualizar(int id, VentaDto dto)
    {
        var venta = await _context.Ventas.FindAsync(id);

        if (venta == null)
            return false;

        venta.ClienteId = dto.ClienteId;
        venta.EmpleadoId = dto.EmpleadoId;
        venta.Fecha = dto.Fecha;
        venta.Total = dto.Total;
        venta.Subtotal = dto.Subtotal;
        venta.Itbis = dto.Itbis;
        venta.ComprobanteFiscal = dto.ComprobanteFiscal;
        venta.MetodoPagoId = dto.MetodoPagoId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var venta = await _context.Ventas.FindAsync(id);

        if (venta == null)
            return false;

        _context.Ventas.Remove(venta);
        await _context.SaveChangesAsync();
        return true;
    }
}
