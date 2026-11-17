
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using tienda.Models;

//namespace tienda.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class DetallesVentaController : ControllerBase
//{
//    private readonly TiendaContext _context;

//    public DetallesVentaController(TiendaContext context)
//    {
//        _context = context;
//    }

//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<DetalleVentum>>> GetDetalles()
//    {
//        return await _context.DetalleVenta
//            .Include(d => d.IdProductoNavigation)
//            .Include(d => d.IdVentaNavigation)
//            .ToListAsync();
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<DetalleVentum>> GetDetalle(int id)
//    {
//        var detalle = await _context.DetalleVenta
//            .Include(d => d.IdProductoNavigation)
//            .Include(d => d.IdVentaNavigation)
//            .FirstOrDefaultAsync(d => d.IdDetalleVenta == id);

//        if (detalle == null)
//            return NotFound();

//        return detalle;
//    }

//    [HttpPost]
//    public async Task<ActionResult<DetalleVentum>> CrearDetalle(DetalleVentum detalle)
//    {
//        _context.DetalleVenta.Add(detalle);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction(nameof(GetDetalle),
//            new { id = detalle.IdDetalleVenta }, detalle);
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> ActualizarDetalle(int id, DetalleVentum detalle)
//    {
//        if (id != detalle.IdDetalleVenta)
//            return BadRequest();

//        var existente = await _context.DetalleVenta.FindAsync(id);

//        if (existente == null)
//            return NotFound();

//        existente.IdVenta = detalle.IdVenta;
//        existente.IdProducto = detalle.IdProducto;
//        existente.Cantidad = detalle.Cantidad;
//        existente.PrecioUnitario = detalle.PrecioUnitario;
//        existente.Subtotal = detalle.Subtotal;

//        await _context.SaveChangesAsync();

//        return NoContent();
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> EliminarDetalle(int id)
//    {
//        var detalle = await _context.DetalleVenta.FindAsync(id);

//        if (detalle == null)
//            return NotFound();

//        _context.DetalleVenta.Remove(detalle);
//        await _context.SaveChangesAsync();

//        return NoContent();
//    }
//}
