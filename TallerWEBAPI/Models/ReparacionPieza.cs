using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Models;

public partial class ReparacionPieza
{
    public int IdReparacion { get; set; }

    public int IdPieza { get; set; }

    public int CantidadUsada { get; set; }
}
