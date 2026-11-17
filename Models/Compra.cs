using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class Compra
{
    public int CompraId { get; set; }

    public int ProveedorId { get; set; }

    public int EmpleadoId { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal Total { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();

    public virtual Empleado Empleado { get; set; } = null!;

    public virtual Proveedor Proveedor { get; set; } = null!;
}
