
namespace tienda.Models;

public class VentaDetalle
{
    public int VentaDetalleId { get; set; }
    public int VentaId { get; set; }
    public int ProductoId { get; set; }
    public int? Cantidad { get; set; }
    public decimal Precio { get; set; }
    public decimal Costo { get; set; }
    public decimal Itbis { get; set; }

    public virtual Venta Venta { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
}
