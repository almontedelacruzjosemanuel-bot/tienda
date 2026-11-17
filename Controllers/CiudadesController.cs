
using Microsoft.AspNetCore.Mvc;
using tienda.DTOs;
using tienda.Services;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CiudadesController : ControllerBase
{
    private readonly CiudadService _service;

    public CiudadesController(CiudadService service)
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
        var ciudad = await _service.ObtenerPorId(id);

        if (ciudad == null)
            return NotFound();

        return Ok(ciudad);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CiudadDto dto)
    {
        var ciudad = await _service.Crear(dto);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = ciudad.CiudadId }, ciudad);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, CiudadDto dto)
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
