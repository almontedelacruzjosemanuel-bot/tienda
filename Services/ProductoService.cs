
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

    public async Task<List<ProductoDto>> ObtenerTodos()
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

    public async Task<ProductoDto> Crear(ProductoDto dto)
    {
        var producto = new Producto
        {
            CategoriaId = dto.CategoriaId,
            MarcaId = dto.MarcaId,
            Sku = dto.Sku,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Costo = dto.Costo,
            Precio = dto.Precio,
            Stock = dto.Stock,
            PuntoReorden = dto.PuntoReorden,
            Garantia = dto.Garantia,
            MargenGanancia = dto.MargenGanancia,
            Itbis = dto.Itbis
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        dto.ProductoId = producto.ProductoId;

        return dto;
    }

    public async Task<bool> Actualizar(int id, ProductoDto dto)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
            return false;

        producto.CategoriaId = dto.CategoriaId;
        producto.MarcaId = dto.MarcaId;
        producto.Sku = dto.Sku;
        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.Costo = dto.Costo;
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;
        producto.PuntoReorden = dto.PuntoReorden;
        producto.Garantia = dto.Garantia;
        producto.MargenGanancia = dto.MargenGanancia;
        producto.Itbis = dto.Itbis;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
            return false;

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();

        return true;
    }
}
