
namespace tienda.Models;

public class MetodoPago
{
    public int MetodoPagoId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}
