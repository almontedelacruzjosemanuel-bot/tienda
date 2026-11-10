using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string? NombreEmpresa { get; set; }

    public string? ContactoNombre { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<OrdenCompra> OrdenCompras { get; set; } = new List<OrdenCompra>();
}
