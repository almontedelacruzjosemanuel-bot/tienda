
using BonaoSells.Application.Services;
using Microsoft.AspNetCore.Mvc;
using BonaoSells.Application.DTOs;

namespace BonaoSells.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly CategoriaService _service;

    public CategoriasController(CategoriaService service)
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
        var categoria = await _service.ObtenerPorId(id);

        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CategoriaDto dto)
    {
        var categoria = await _service.Crear(dto);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, CategoriaDto dto)
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
