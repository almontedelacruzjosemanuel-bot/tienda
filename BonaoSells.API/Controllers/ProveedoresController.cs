
using BonaoSells.Application.Services;
using Microsoft.AspNetCore.Mvc;
using BonaoSells.Application.DTOs;

namespace BonaoSells.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProveedoresController : ControllerBase
{
    private readonly ProveedorService _service;

    public ProveedoresController(ProveedorService service)
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
        var proveedor = await _service.ObtenerPorId(id);

        if (proveedor == null)
            return NotFound();

        return Ok(proveedor);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(ProveedorDto dto)
    {
        var proveedor = await _service.Crear(dto);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = proveedor.ProveedorId }, proveedor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, ProveedorDto dto)
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
