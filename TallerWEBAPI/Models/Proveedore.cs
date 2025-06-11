using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Models;

public partial class Proveedore
{
    public int IdProveedor { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? Correo { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
}
