
namespace tienda.DTOs;

public class EmpleadoDto
{
    public int IdEmpleado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Cargo { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public DateOnly? FechaContratacion { get; set; }
}
