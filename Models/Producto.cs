using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public int CategoriaId { get; set; }

    public int MarcaId { get; set; }

    public string? Sku { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal Costo { get; set; }

    public decimal Precio { get; set; }

    public int? Stock { get; set; }

    public int? PuntoReorden { get; set; }

    public string? Garantia { get; set; }

    public decimal? MargenGanancia { get; set; }

    public decimal? Itbis { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();

    public virtual ICollection<EspecificacionesTecnica> EspecificacionesTecnicas { get; set; } = new List<EspecificacionesTecnica>();

    public virtual Marca Marca { get; set; } = null!;

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
