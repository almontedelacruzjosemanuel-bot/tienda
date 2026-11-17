
using Microsoft.AspNetCore.Mvc;
using tienda.DTOs;
using tienda.Services;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetodoPagosController : ControllerBase
{
    private readonly MetodoPagoService _service;

    public MetodoPagosController(MetodoPagoService service)
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
        var metodoPago = await _service.ObtenerPorId(id);

        if (metodoPago == null)
            return NotFound();

        return Ok(metodoPago);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(MetodoPagoDto dto)
    {
        var metodoPago = await _service.Crear(dto);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = metodoPago.MetodoPagoId }, metodoPago);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, MetodoPagoDto dto)
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
