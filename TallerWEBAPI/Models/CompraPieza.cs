using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Models;

public partial class CompraPieza
{
    public int IdCompra { get; set; }

    public int IdPieza { get; set; }

    public int CantidadComprada { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual Inventario IdPiezaNavigation { get; set; } = null!;
}
