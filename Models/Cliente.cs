using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? DocumentoIdentidad { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
