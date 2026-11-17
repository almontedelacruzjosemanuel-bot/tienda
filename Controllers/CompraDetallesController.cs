
using Microsoft.AspNetCore.Mvc;
using tienda.DTOs;
using tienda.Services;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompraDetallesController : ControllerBase
{
    private readonly CompraDetalleService _service;

    public CompraDetallesController(CompraDetalleService service)
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
        var detalle = await _service.ObtenerPorId(id);

        if (detalle == null)
            return NotFound();

        return Ok(detalle);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CompraDetalleDto dto)
    {
        var detalle = await _service.Crear(dto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = detalle.CompraDetalleId },
            detalle
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, CompraDetalleDto dto)
    {
        if (!await _service.Actualizar(id, dto))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        if (!await _service.Eliminar(id))
            return NotFound();

        return NoContent();
    }
}
