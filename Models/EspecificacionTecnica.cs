using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class EspecificacionTecnica
{
    public int IdEspecificacion { get; set; }

    public int IdProducto { get; set; }

    public string? Clave { get; set; }

    public string? Valor { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
