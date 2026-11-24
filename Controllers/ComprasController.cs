
using Microsoft.AspNetCore.Mvc;
using tienda.DTOs;
using tienda.Services;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComprasController : ControllerBase
{
    private readonly CompraService _service;

    public ComprasController(CompraService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodas()
    {
        return Ok(await _service.ObtenerTodas());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var compra = await _service.ObtenerPorId(id);

        if (compra == null)
            return NotFound();

        return Ok(compra);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CompraDto dto)
    {
        var compra = await _service.Crear(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = compra.CompraId }, compra);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, CompraDto dto)
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
