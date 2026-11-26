namespace BonaoSells.Infrastructure.Models;

public class EspecificacionTecnica
{
    public int EspecificacionId { get; set; }

    public int ProductoId { get; set; }

    public string? Clave { get; set; }

    public string? Valor { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
