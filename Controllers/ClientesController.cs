
using Microsoft.AspNetCore.Mvc;
using tienda.DTOs;
using tienda.Services;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _service;

    public ClientesController(ClienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(await _service.ObtenerTodos());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var cliente = await _service.ObtenerPorId(id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(ClienteDto dto)
    {
        var cliente = await _service.Crear(dto);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.ClienteId }, cliente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, ClienteDto dto)
    {
        var actualizado = await _service.Actualizar(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _service.Eliminar(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}
