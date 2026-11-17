
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using tienda.Models;

//namespace tienda.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class CategoriasController : ControllerBase
//{
//    private readonly TiendaContext _context;

//    public CategoriasController(TiendaContext context)
//    {
//        _context = context;
//    }

//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<Categorium>>> GetCategorias()
//    {
//        return await _context.Categorias.ToListAsync();
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<Categorium>> GetCategoria(int id)
//    {
//        var categoria = await _context.Categorias.FindAsync(id);

//        if (categoria == null)
//            return NotFound();

//        return categoria;
//    }

//    [HttpPost]
//    public async Task<ActionResult<Categorium>> CrearCategoria(Categorium categoria)
//    {
//        _context.Categorias.Add(categoria);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction(nameof(GetCategoria),
//            new { id = categoria.IdCategoria }, categoria);
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> ActualizarCategoria(int id, Categorium categoria)
//    {
//        if (id != categoria.IdCategoria)
//            return BadRequest();

//        var existente = await _context.Categorias.FindAsync(id);

//        if (existente == null)
//            return NotFound();

//        existente.Nombre = categoria.Nombre;
//        existente.Descripcion = categoria.Descripcion;

//        await _context.SaveChangesAsync();

//        return NoContent();
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> EliminarCategoria(int id)
//    {
//        var categoria = await _context.Categorias.FindAsync(id);

//        if (categoria == null)
//            return NotFound();

//        _context.Categorias.Remove(categoria);
//        await _context.SaveChangesAsync();

//        return NoContent();
//    }
//}
