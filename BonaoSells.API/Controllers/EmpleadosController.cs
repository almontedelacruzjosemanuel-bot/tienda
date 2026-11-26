
using BonaoSells.Application.Services;
using Microsoft.AspNetCore.Mvc;
using BonaoSells.Application.DTOs;

namespace BonaoSells.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmpleadosController : ControllerBase
{
    private readonly EmpleadoService _service;

    public EmpleadosController(EmpleadoService service)
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
        var empleado = await _service.ObtenerPorId(id);

        if (empleado == null)
            return NotFound();

        return Ok(empleado);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(EmpleadoDto dto)
    {
        var empleado = await _service.Crear(dto);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = empleado.EmpleadoId }, empleado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, EmpleadoDto dto)
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
