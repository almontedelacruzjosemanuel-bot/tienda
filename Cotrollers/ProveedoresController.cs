
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda.Models;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProveedoresController : ControllerBase
{
    private readonly TiendaContext _context;

    public ProveedoresController(TiendaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores()
    {
        return await _context.Proveedors.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Proveedor>> GetProveedor(int id)
    {
        var proveedor = await _context.Proveedors.FindAsync(id);

        if (proveedor == null)
            return NotFound();

        return proveedor;
    }

    [HttpPost]
    public async Task<ActionResult<Proveedor>> CrearProveedor(Proveedor proveedor)
    {
        _context.Proveedors.Add(proveedor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProveedor),
            new { id = proveedor.IdProveedor }, proveedor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarProveedor(int id, Proveedor proveedor)
    {
        if (id != proveedor.IdProveedor)
            return BadRequest();

        var existente = await _context.Proveedors.FindAsync(id);

        if (existente == null)
            return NotFound();

        existente.NombreEmpresa = proveedor.NombreEmpresa;
        existente.ContactoNombre = proveedor.ContactoNombre;
        existente.Telefono = proveedor.Telefono;
        existente.Email = proveedor.Email;
        existente.Direccion = proveedor.Direccion;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarProveedor(int id)
    {
        var proveedor = await _context.Proveedors.FindAsync(id);

        if (proveedor == null)
            return NotFound();

        _context.Proveedors.Remove(proveedor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
