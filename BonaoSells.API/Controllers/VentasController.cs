
using BonaoSells.Application.Services;
using Microsoft.AspNetCore.Mvc;
using BonaoSells.Application.DTOs;

namespace BonaoSells.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VentasController : ControllerBase
{
    private readonly VentaService _service;

    public VentasController(VentaService service)
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
        var venta = await _service.ObtenerPorId(id);

        if (venta == null)
            return NotFound();

        return Ok(venta);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(VentaDto dto)
    {
        var venta = await _service.Crear(dto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = venta.VentaId },
            venta
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, VentaDto dto)
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
