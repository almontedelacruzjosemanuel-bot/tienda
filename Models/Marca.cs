using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class Marca
{
    public int IdMarca { get; set; }

    public string? Nombre { get; set; }

    public string? PaisOrigen { get; set; }

    public string? SitioWeb { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
