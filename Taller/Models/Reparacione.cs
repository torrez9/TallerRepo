using System;
using System.Collections.Generic;

namespace Taller.Models;

public partial class Reparacione
{
    public int IdReparacion { get; set; }

    public int IdMoto { get; set; }

    public int IdUsuario { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? Estado { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public decimal? Total { get; set; }

    public virtual ICollection<DetalleReparacion> DetalleReparacions { get; set; } = new List<DetalleReparacion>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Moto IdMotoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<ReparacionPieza> ReparacionPiezas { get; set; } = new List<ReparacionPieza>();
}
