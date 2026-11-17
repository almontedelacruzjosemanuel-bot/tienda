using System;
using System.Collections.Generic;

namespace tienda.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public int EmpleadoId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ContrasenaHash { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;
}
