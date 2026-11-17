
using Microsoft.EntityFrameworkCore;
using tienda.DTOs;
using tienda.Models;

namespace tienda.Services;

public class ProductoService
{
    private readonly TiendaContext _context;

    public ProductoService(TiendaContext context)
    {
        _context = context;
    }

    public async Task<List<ProductoDto>> ObtenerTodas()
    {
        return await _context.Productos
            .Select(p => new ProductoDto
            {
                ProductoId = p.ProductoId,
                CategoriaId = p.CategoriaId,
                MarcaId = p.MarcaId,
                Sku = p.Sku,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Costo = p.Costo,
                Precio = p.Precio,
                Stock = p.Stock,
                PuntoReorden = p.PuntoReorden,
                Garantia = p.Garantia,
                MargenGanancia = p.MargenGanancia,
                Itbis = p.Itbis
            })
            .ToListAsync();
    }

    public async Task<ProductoDto?> ObtenerPorId(int id)
    {
        return await _context.Productos
            .Where(p => p.ProductoId == id)
            .Select(p => new ProductoDto
            {
                ProductoId = p.ProductoId,
                CategoriaId = p.CategoriaId,
                MarcaId = p.MarcaId,
                Sku = p.Sku,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Costo = p.Costo,
                Precio = p.Precio,
                Stock = p.Stock,
                PuntoReorden = p.PuntoReorden,
                Garantia = p.Garantia,
                MargenGanancia = p.MargenGanancia,
                Itbis = p.Itbis
            })
            .FirstOrDefaultAsync();
    }
}
