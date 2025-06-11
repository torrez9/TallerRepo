using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Models;

public partial class Inventario
{
    public int IdPieza { get; set; }

    public string NombrePieza { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal PrecioActual { get; set; }

    public int? IdProveedor { get; set; }

    public virtual ICollection<CompraPieza> CompraPiezas { get; set; } = new List<CompraPieza>();

    public virtual ICollection<DetalleReparacion> DetalleReparacions { get; set; } = new List<DetalleReparacion>();

    public virtual Proveedore? IdProveedorNavigation { get; set; }
}
