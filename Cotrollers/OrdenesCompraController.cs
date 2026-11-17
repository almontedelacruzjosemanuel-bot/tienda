
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using tienda.Models;

//namespace tienda.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class OrdenesCompraController : ControllerBase
//{
//    private readonly TiendaContext _context;

//    public OrdenesCompraController(TiendaContext context)
//    {
//        _context = context;
//    }

//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<OrdenCompra>>> GetOrdenes()
//    {
//        return await _context.OrdenCompras
//            .Include(o => o.IdProveedorNavigation)
//            .Include(o => o.IdEmpleadoNavigation)
//            .ToListAsync();
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<OrdenCompra>> GetOrden(int id)
//    {
//        var orden = await _context.OrdenCompras
//            .Include(o => o.IdProveedorNavigation)
//            .Include(o => o.IdEmpleadoNavigation)
//            .Include(o => o.DetalleOrdenCompras)
//            .FirstOrDefaultAsync(o => o.IdOrdenCompra == id);

//        if (orden == null)
//            return NotFound();

//        return orden;
//    }

//    [HttpPost]
//    public async Task<ActionResult<OrdenCompra>> CrearOrden(OrdenCompra orden)
//    {
//        _context.OrdenCompras.Add(orden);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction(nameof(GetOrden),
//            new { id = orden.IdOrdenCompra }, orden);
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> ActualizarOrden(int id, OrdenCompra orden)
//    {
//        if (id != orden.IdOrdenCompra)
//            return BadRequest();

//        var existente = await _context.OrdenCompras.FindAsync(id);

//        if (existente == null)
//            return NotFound();

//        existente.IdProveedor = orden.IdProveedor;
//        existente.IdEmpleado = orden.IdEmpleado;
//        existente.FechaOrden = orden.FechaOrden;
//        existente.Total = orden.Total;
//        existente.Estado = orden.Estado;

//        await _context.SaveChangesAsync();

//        return NoContent();
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> EliminarOrden(int id)
//    {
//        var orden = await _context.OrdenCompras.FindAsync(id);

//        if (orden == null)
//            return NotFound();

//        _context.OrdenCompras.Remove(orden);
//        await _context.SaveChangesAsync();

//        return NoContent();
//    }
//}
