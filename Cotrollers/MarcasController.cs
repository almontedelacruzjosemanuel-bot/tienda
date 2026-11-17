
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda.Models;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarcasController : ControllerBase
{
    private readonly TiendaContext _context;

    public MarcasController(TiendaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Marca>>> GetMarcas()
    {
        return await _context.Marcas.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Marca>> GetMarca(int id)
    {
        var marca = await _context.Marcas.FindAsync(id);

        if (marca == null)
            return NotFound();

        return marca;
    }

    [HttpPost]
    public async Task<ActionResult<Marca>> CrearMarca(Marca marca)
    {
        _context.Marcas.Add(marca);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMarca),
            new { id = marca.IdMarca }, marca);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarMarca(int id, Marca marca)
    {
        if (id != marca.IdMarca)
            return BadRequest();

        var existente = await _context.Marcas.FindAsync(id);

        if (existente == null)
            return NotFound();

        existente.Nombre = marca.Nombre;
        existente.PaisOrigen = marca.PaisOrigen;
        existente.SitioWeb = marca.SitioWeb;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarMarca(int id)
    {
        var marca = await _context.Marcas.FindAsync(id);

        if (marca == null)
            return NotFound();

        _context.Marcas.Remove(marca);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
