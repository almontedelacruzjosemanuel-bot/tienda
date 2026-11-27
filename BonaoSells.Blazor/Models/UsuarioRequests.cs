using BonaoSells.Application.DTOs;

namespace BonaoSells.Blazor.Models;

public class CrearUsuarioRequest
{
    public UsuarioDto Usuario { get; set; } = new();
    public string Contrasena { get; set; } = string.Empty;
}

public class CambiarContrasenaRequest
{
    public string NuevaContrasena { get; set; } = string.Empty;
}
