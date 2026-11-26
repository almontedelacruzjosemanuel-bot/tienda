namespace BonaoSells.Application.DTOs;

public class VentaDto
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
}
