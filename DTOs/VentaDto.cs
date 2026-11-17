
namespace tienda.DTOs;

public class VentaDto
{
    public int IdVenta { get; set; }

    public int IdCliente { get; set; }

    public int IdEmpleado { get; set; }

    public DateTime FechaVenta { get; set; }

    public decimal Total { get; set; }

    public string? MetodoPago { get; set; }

    public string? Estado { get; set; }
}

