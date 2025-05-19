using System;
using System.Collections.Generic;

namespace Taller.Models;

public partial class Inventario
{
    public int IdPieza { get; set; }

    public string NombrePieza { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public virtual ICollection<CompraPieza> CompraPiezas { get; set; } = new List<CompraPieza>();

    public virtual ICollection<ReparacionPieza> ReparacionPiezas { get; set; } = new List<ReparacionPieza>();
}
