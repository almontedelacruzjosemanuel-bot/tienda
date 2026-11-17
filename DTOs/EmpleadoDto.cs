
namespace tienda.DTOs;

public class EmpleadoDto
{
    public int EmpleadoId { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Cargo { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public DateOnly? FechaContratacion { get; set; }
}
