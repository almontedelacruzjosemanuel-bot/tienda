
namespace tienda.Models;

public class Empleado
{
    public int EmpleadoId { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Cargo { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public DateOnly? FechaContratacion { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
