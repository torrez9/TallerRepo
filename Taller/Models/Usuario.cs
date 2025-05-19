using System;
using System.Collections.Generic;

namespace Taller.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Apellido { get; set; }

    public int? Telefono { get; set; }

    public string Correo { get; set; } = null!;

    public string? Usuario1 { get; set; }

    public string Contraseña { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Reparacione> Reparaciones { get; set; } = new List<Reparacione>();
}
