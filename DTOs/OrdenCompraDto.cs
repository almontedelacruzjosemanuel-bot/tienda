
namespace tienda.DTOs;

public class DetalleOrdenCompraDto
{
    public int CompraDetalleId { get; set; }

    public int CompraId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal Subtotal { get; set; }
}
