
namespace tienda.Models;

public class Cliente
{
    public int ClienteId { get; set; }

    public string? DocumentoIdentidad { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public string? NombreCompleto { get; set; }

    public int? CiudadId { get; set; }

    public virtual Ciudad? Ciudad { get; set; }

    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}
