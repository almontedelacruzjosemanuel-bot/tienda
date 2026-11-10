using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class Ventum
{
    public int IdVenta { get; set; }

    public int IdCliente { get; set; }

    public int IdEmpleado { get; set; }

    public DateTime? FechaVenta { get; set; }

    public decimal? Total { get; set; }

    public string? MetodoPago { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
}
