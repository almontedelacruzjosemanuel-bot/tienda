namespace BonaoSells.Infrastructure.Models;

public class Venta
{
    public int VentaId { get; set; }
    public int ClienteId { get; set; }
    public int EmpleadoId { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Itbis { get; set; }
    public string? ComprobanteFiscal { get; set; }
    public int? MetodoPagoId { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;
    public virtual Empleado Empleado { get; set; } = null!;
    public virtual MetodoPago? MetodoPago { get; set; }
    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
