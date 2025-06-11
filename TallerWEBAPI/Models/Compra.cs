using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public int? IdProveedor { get; set; }

    public DateTime? FechaCompra { get; set; }

    public decimal MontoTotal { get; set; }

    public virtual ICollection<CompraPieza> CompraPiezas { get; set; } = new List<CompraPieza>();

    public virtual Proveedore? IdProveedorNavigation { get; set; }
}
