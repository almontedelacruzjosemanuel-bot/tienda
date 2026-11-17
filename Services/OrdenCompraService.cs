
using Microsoft.EntityFrameworkCore;
using tienda.DTOs;
using tienda.Models;

namespace tienda.Services;

public class OrdenCompraService
{
    private readonly TiendaContext _context;

    public OrdenCompraService(TiendaContext context)
    {
        _context = context;
    }

    public async Task<List<OrdenCompraDto>> ObtenerTodas()
    {
        return await _context.OrdenCompras
            .Select(o => new OrdenCompraDto
            {
                IdOrdenCompra = o.IdOrdenCompra,
                IdProveedor = o.IdProveedor,
                IdEmpleado = o.IdEmpleado,
                FechaOrden = o.FechaOrden,
                Total = o.Total,
                Estado = o.Estado
            })
            .ToListAsync();
    }

    public async Task<DetalleOrdenCompraDto?> ObtenerPorId(int id)
    {
        return await _context.OrdenCompras
            .Where(o => o.IdOrdenCompra == id)
            .Select(o => new OrdenCompraDto
            {
                IdOrdenCompra = o.IdOrdenCompra,
                IdProveedor = o.IdProveedor,
                IdEmpleado = o.IdEmpleado,
                FechaOrden = o.FechaOrden,
                Total = o.Total,
                Estado = o.Estado
            })
            .FirstOrDefaultAsync();
    }
}
