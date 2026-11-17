
namespace tienda.Models;

public class Producto
{
    public int ProductoId { get; set; }

    public int CategoriaId { get; set; }

    public int MarcaId { get; set; }

    public string Sku { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Costo { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public int PuntoReorden { get; set; }

    public string? Garantia { get; set; }

    public decimal MargenGanancia { get; set; }

    public decimal Itbis { get; set; }

    public virtual Categorium Categoria { get; set; } = null!;

    public virtual Marca Marca { get; set; } = null!;

    public virtual ICollection<DetalleOrdenCompra> DetalleOrdenCompras { get; set; } = new List<DetalleOrdenCompra>();

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual ICollection<EspecificacionTecnica> EspecificacionTecnicas { get; set; } = new List<EspecificacionTecnica>();
}
