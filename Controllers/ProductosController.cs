
using Microsoft.AspNetCore.Mvc;
using tienda.DTOs;
using tienda.Services;

namespace tienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ProductoService _service;

    public ProductosController(ProductoService service)
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
        var producto = await _service.ObtenerPorId(id);

        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(ProductoDto dto)
    {
        var producto = await _service.Crear(dto);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.ProductoId }, producto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, ProductoDto dto)
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
