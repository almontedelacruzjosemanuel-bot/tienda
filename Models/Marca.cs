
namespace tienda.Models;

public class Marca
{
    public int MarcaId { get; set; }

    public string? Nombre { get; set; }

    public string? PaisOrigen { get; set; }

    public string? SitioWeb { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
