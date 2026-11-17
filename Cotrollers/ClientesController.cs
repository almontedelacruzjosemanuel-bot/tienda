
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda.Models;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly TiendaContext _context;

    public ClientesController(TiendaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
    {
        return await _context.Clientes.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound();

        return cliente;
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> CrearCliente(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCliente),
            new { id = cliente.IdCliente }, cliente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarCliente(int id, Cliente cliente)
    {
        if (id != cliente.IdCliente)
            return BadRequest();

        var existente = await _context.Clientes.FindAsync(id);

        if (existente == null)
            return NotFound();

        existente.Nombre = cliente.Nombre;
        existente.Apellido = cliente.Apellido;
        existente.DocumentoIdentidad = cliente.DocumentoIdentidad;
        existente.Email = cliente.Email;
        existente.Telefono = cliente.Telefono;
        existente.Direccion = cliente.Direccion;
        existente.FechaRegistro = cliente.FechaRegistro;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound();

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
