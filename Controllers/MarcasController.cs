
using Microsoft.AspNetCore.Mvc;
using tienda.DTOs;
using tienda.Services;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarcasController : ControllerBase
{
    private readonly MarcaService _service;

    public MarcasController(MarcaService service)
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
        var marca = await _service.ObtenerPorId(id);

        if (marca == null)
            return NotFound();

        return Ok(marca);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(MarcaDto dto)
    {
        var marca = await _service.Crear(dto);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = marca.MarcaId }, marca);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, MarcaDto dto)
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
