using System;
using System.Collections.Generic;

namespace Taller.Models;

public partial class Factura
{
    public int IdFactura { get; set; }

    public int IdCliente { get; set; }

    public int IdReparacion { get; set; }

    public DateTime? FechaFactura { get; set; }

    public decimal MontoTotal { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Reparacione IdReparacionNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
