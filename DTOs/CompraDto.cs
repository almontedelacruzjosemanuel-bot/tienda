
namespace tienda.DTOs;

public class CompraDto
{
    public int CompraId { get; set; }
    public int ProveedorId { get; set; }
    public int EmpleadoId { get; set; }
    public DateTime? Fecha { get; set; }
    public decimal Total { get; set; }
    public string? Estado { get; set; }
}
