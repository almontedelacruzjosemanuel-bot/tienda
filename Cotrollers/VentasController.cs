
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda.Models;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VentasController : ControllerBase
{
    private readonly TiendaContext _context;

    public VentasController(TiendaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ventum>>> GetVentas()
    {
        return await _context.Venta
            .Include(v => v.IdClienteNavigation)
            .Include(v => v.IdEmpleadoNavigation)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ventum>> GetVenta(int id)
    {
        var venta = await _context.Venta
            .Include(v => v.IdClienteNavigation)
            .Include(v => v.IdEmpleadoNavigation)
            .Include(v => v.DetalleVenta)
            .FirstOrDefaultAsync(v => v.IdVenta == id);

        if (venta == null)
            return NotFound();

        return venta;
    }

    [HttpPost]
    public async Task<ActionResult<Ventum>> CrearVenta(Ventum venta)
    {
        _context.Venta.Add(venta);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVenta),
            new { id = venta.IdVenta }, venta);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarVenta(int id, Ventum venta)
    {
        if (id != venta.IdVenta)
            return BadRequest();

        var existente = await _context.Venta.FindAsync(id);

        if (existente == null)
            return NotFound();

        existente.IdCliente = venta.IdCliente;
        existente.IdEmpleado = venta.IdEmpleado;
        existente.FechaVenta = venta.FechaVenta;
        existente.Total = venta.Total;
        existente.MetodoPago = venta.MetodoPago;
        existente.Estado = venta.Estado;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarVenta(int id)
    {
        var venta = await _context.Venta.FindAsync(id);

        if (venta == null)
            return NotFound();

        _context.Venta.Remove(venta);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
