using System;
using System.Collections.Generic;

namespace Taller.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public int IdFactura { get; set; }

    public string MetodoPago { get; set; } = null!;

    public decimal Monto { get; set; }

    public DateTime? FechaPago { get; set; }

    public virtual Factura IdFacturaNavigation { get; set; } = null!;
}
