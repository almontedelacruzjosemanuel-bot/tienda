using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class DetalleOrdenCompra
{
    public int IdDetalleCompra { get; set; }

    public int IdOrdenCompra { get; set; }

    public int IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? CostoUnitario { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual OrdenCompra IdOrdenCompraNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
