using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int IdCategoria { get; set; }

    public int IdMarca { get; set; }

    public string? CodigoSku { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? PrecioCompra { get; set; }

    public decimal? PrecioVenta { get; set; }

    public int? StockActual { get; set; }

    public int? StockMinimo { get; set; }

    public string? GarantiaMeses { get; set; }

    public virtual ICollection<DetalleOrdenCompra> DetalleOrdenCompras { get; set; } = new List<DetalleOrdenCompra>();

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual ICollection<EspecificacionTecnica> EspecificacionTecnicas { get; set; } = new List<EspecificacionTecnica>();

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual Marca IdMarcaNavigation { get; set; } = null!;
}
