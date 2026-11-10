using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class OrdenCompra
{
    public int IdOrdenCompra { get; set; }

    public int IdProveedor { get; set; }

    public int IdEmpleado { get; set; }

    public DateTime? FechaOrden { get; set; }

    public decimal? Total { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<DetalleOrdenCompra> DetalleOrdenCompras { get; set; } = new List<DetalleOrdenCompra>();

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
}
