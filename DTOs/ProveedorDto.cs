
namespace tienda.DTOs;

public class ProveedorDto
{
    public int IdProveedor { get; set; }

    public string NombreEmpresa { get; set; } = null!;

    public string? ContactoNombre { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }
}
