
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using tienda.Models;

//namespace tienda.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class ProductosController : ControllerBase
//{
//    private readonly TiendaContext _context;

//    public ProductosController(TiendaContext context)
//    {
//        _context = context;
//    }

//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
//    {
//        return await _context.Productos.ToListAsync();
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<Producto>> GetProducto(int id)
//    {
//        var producto = await _context.Productos.FindAsync(id);

//        if (producto == null)
//            return NotFound();

//        return producto;
//    }

//    [HttpPost]
//    public async Task<ActionResult<Producto>> CrearProducto(Producto producto)
//    {
//        _context.Productos.Add(producto);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction(nameof(GetProducto),
//            new { id = producto.ProductoId }, producto);
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
//    {
//        if (id != producto.ProductoId)
//            return BadRequest();

//        var existente = await _context.Productos.FindAsync(id);

//        if (existente == null)
//            return NotFound();

//        existente.CategoriaId = producto.CategoriaId;
//        existente.MarcaId = producto.MarcaId;
//        existente.Sku = producto.Sku;
//        existente.Nombre = producto.Nombre;
//        existente.Descripcion = producto.Descripcion;
//        existente.Costo = producto.Costo;
//        existente.Precio = producto.Precio;
//        existente.Stock = producto.Stock;
//        existente.PuntoReorden = producto.PuntoReorden;
//        existente.Garantia = producto.Garantia;
//        existente.MargenGanancia = producto.MargenGanancia;
//        existente.Itbis = producto.Itbis;

//        await _context.SaveChangesAsync();

//        return NoContent();
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> EliminarProducto(int id)
//    {
//        var producto = await _context.Productos.FindAsync(id);

//        if (producto == null)
//            return NotFound();

//        _context.Productos.Remove(producto);
//        await _context.SaveChangesAsync();

//        return NoContent();
//    }
//}
