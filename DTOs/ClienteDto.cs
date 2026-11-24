
namespace tienda.DTOs;

public class ClienteDto
{
    public int ClienteId { get; set; }

    public string? DocumentoIdentidad { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public string? NombreCompleto { get; set; }

    public int? CiudadId { get; set; }
}
