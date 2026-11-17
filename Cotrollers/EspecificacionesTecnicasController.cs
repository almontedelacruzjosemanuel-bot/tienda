
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda.Models;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EspecificacionesTecnicasController : ControllerBase
{
    private readonly TiendaContext _context;

    public EspecificacionesTecnicasController(TiendaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EspecificacionTecnica>>> GetEspecificaciones()
    {
        return await _context.EspecificacionTecnicas
            .Include(e => e.IdProductoNavigation)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EspecificacionTecnica>> GetEspecificacion(int id)
    {
        var especificacion = await _context.EspecificacionTecnicas
            .Include(e => e.IdProductoNavigation)
            .FirstOrDefaultAsync(e => e.IdEspecificacion == id);

        if (especificacion == null)
            return NotFound();

        return especificacion;
    }

    [HttpPost]
    public async Task<ActionResult<EspecificacionTecnica>> CrearEspecificacion(EspecificacionTecnica especificacion)
    {
        _context.EspecificacionTecnicas.Add(especificacion);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEspecificacion),
            new { id = especificacion.IdEspecificacion }, especificacion);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarEspecificacion(int id, EspecificacionTecnica especificacion)
    {
        if (id != especificacion.IdEspecificacion)
            return BadRequest();

        var existente = await _context.EspecificacionTecnicas.FindAsync(id);

        if (existente == null)
            return NotFound();

        existente.IdProducto = especificacion.IdProducto;
        existente.Clave = especificacion.Clave;
        existente.Valor = especificacion.Valor;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarEspecificacion(int id)
    {
        var especificacion = await _context.EspecificacionTecnicas.FindAsync(id);

        if (especificacion == null)
            return NotFound();

        _context.EspecificacionTecnicas.Remove(especificacion);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
