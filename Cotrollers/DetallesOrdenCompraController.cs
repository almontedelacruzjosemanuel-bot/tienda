
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda.Models;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DetallesOrdenCompraController : ControllerBase
{
    private readonly TiendaContext _context;

    public DetallesOrdenCompraController(TiendaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DetalleOrdenCompra>>> GetDetalles()
    {
        return await _context.DetalleOrdenCompras
            .Include(d => d.Producto)
            .Include(d => d.Compra)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DetalleOrdenCompra>> GetDetalle(int id)
    {
        var detalle = await _context.DetalleOrdenCompras
            .Include(d => d.Producto)
            .Include(d => d.Compra)
            .FirstOrDefaultAsync(d => d.CompraDetalleId == id);

        if (detalle == null)
            return NotFound();

        return detalle;
    }

    [HttpPost]
    public async Task<ActionResult<DetalleOrdenCompra>> CrearDetalle(DetalleOrdenCompra detalle)
    {
        _context.DetalleOrdenCompras.Add(detalle);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDetalle),
            new { id = detalle.CompraDetalleId }, detalle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarDetalle(int id, DetalleOrdenCompra detalle)
    {
        if (id != detalle.CompraDetalleId)
            return BadRequest();

        var existente = await _context.DetalleOrdenCompras.FindAsync(id);

        if (existente == null)
            return NotFound();

        existente.CompraId = detalle.CompraId;
        existente.ProductoId = detalle.ProductoId;
        existente.Cantidad = detalle.Cantidad;
        existente.CostoUnitario = detalle.CostoUnitario;
        existente.Subtotal = detalle.Subtotal;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarDetalle(int id)
    {
        var detalle = await _context.DetalleOrdenCompras.FindAsync(id);

        if (detalle == null)
            return NotFound();

        _context.DetalleOrdenCompras.Remove(detalle);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
