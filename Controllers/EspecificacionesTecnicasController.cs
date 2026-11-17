
using Microsoft.AspNetCore.Mvc;
using tienda.DTOs;
using tienda.Services;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EspecificacionesTecnicasController : ControllerBase
{
    private readonly EspecificacionTecnicaService _service;

    public EspecificacionesTecnicasController(EspecificacionTecnicaService service)
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
        var especificacion = await _service.ObtenerPorId(id);

        if (especificacion == null)
            return NotFound();

        return Ok(especificacion);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(EspecificacionTecnicaDto dto)
    {
        var especificacion = await _service.Crear(dto);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = especificacion.EspecificacionId }, especificacion);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, EspecificacionTecnicaDto dto)
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
