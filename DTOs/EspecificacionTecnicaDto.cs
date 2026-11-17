
namespace tienda.DTOs;

public class EspecificacionTecnicaDto
{
    public int IdEspecificacion { get; set; }

    public int IdProducto { get; set; }

    public string Clave { get; set; } = null!;

    public string Valor { get; set; } = null!;
}

