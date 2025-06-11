using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Models;

public partial class CompraPieza
{
    public int IdCompraPieza { get; set; }

    public int IdPieza { get; set; }

    public int? IdCompra { get; set; }

    public int CantidadComprada { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal? Importe { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual Inventario IdPiezaNavigation { get; set; } = null!;
}
