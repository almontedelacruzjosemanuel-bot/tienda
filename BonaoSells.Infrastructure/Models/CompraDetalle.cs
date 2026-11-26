namespace BonaoSells.Infrastructure.Models;

public class CompraDetalle
{
    public int CompraDetalleId { get; set; }
    public int CompraId { get; set; }
    public int ProductoId { get; set; }
    public int? Cantidad { get; set; }
    public decimal CostoUnitario { get; set; }
    public decimal Subtotal { get; set; }

    public virtual Compra Compra { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
}
