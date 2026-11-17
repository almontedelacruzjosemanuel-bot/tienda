using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class Ciudad
{
    public int CiudadId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
