
namespace tienda.DTOs;

public class MarcaDto
{
    public int IdMarca { get; set; }

    public string Nombre { get; set; } = null!;

    public string? PaisOrigen { get; set; }

    public string? SitioWeb { get; set; }
}
