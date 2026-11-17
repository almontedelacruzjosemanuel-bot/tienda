
namespace tienda.DTOs;

public class UsuarioDto
{
    public int UsuarioId { get; set; }

    public int EmpleadoId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public bool Activo { get; set; }
}
