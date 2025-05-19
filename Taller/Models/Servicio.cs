using System;
using System.Collections.Generic;

namespace Taller.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public string? Servicio1 { get; set; }

    public decimal? PrecioServicio { get; set; }

    public virtual ICollection<DetalleReparacion> DetalleReparacions { get; set; } = new List<DetalleReparacion>();
}
