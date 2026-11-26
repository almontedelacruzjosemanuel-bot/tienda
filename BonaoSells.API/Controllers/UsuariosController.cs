
using BonaoSells.Application.Services;
using Microsoft.AspNetCore.Mvc;
using BonaoSells.Application.DTOs;

namespace BonaoSells.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuariosController(UsuarioService service)
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
        var usuario = await _service.ObtenerPorId(id);

        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearUsuarioRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Contrasena))
            return BadRequest("La contraseña es obligatoria.");

        var usuario = await _service.Crear(request.Usuario, request.Contrasena);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = usuario.UsuarioId }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, UsuarioDto dto)
    {
        var actualizado = await _service.Actualizar(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id}/contrasena")]
    public async Task<IActionResult> CambiarContrasena(int id, [FromBody] CambiarContrasenaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NuevaContrasena))
            return BadRequest("La nueva contraseña es obligatoria.");

        var cambiado = await _service.CambiarContrasena(id, request.NuevaContrasena);

        if (!cambiado)
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

public class CrearUsuarioRequest
{
    public UsuarioDto Usuario { get; set; } = null!;

    public string Contrasena { get; set; } = null!;
}

public class CambiarContrasenaRequest
{
    public string NuevaContrasena { get; set; } = null!;
}
