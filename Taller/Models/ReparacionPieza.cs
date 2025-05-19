using System;
using System.Collections.Generic;

namespace Taller.Models;

public partial class ReparacionPieza
{
    public int IdReparacion { get; set; }

    public int IdPieza { get; set; }

    public int CantidadUsada { get; set; }

    public virtual Inventario IdPiezaNavigation { get; set; } = null!;

    public virtual Reparacione IdReparacionNavigation { get; set; } = null!;
}
