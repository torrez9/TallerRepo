using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Models;

public partial class DetalleReparacion
{
    public int IdDetalleRep { get; set; }

    public int? IdReparaciones { get; set; }

    public int? Idservicios { get; set; }

    public decimal? CostoServicio { get; set; }

    public int? IdPieza { get; set; }

    public int? CantPieza { get; set; }

    public decimal? CostoPieza { get; set; }

    public decimal? Importe { get; set; }

    public virtual Inventario? IdPiezaNavigation { get; set; }

    public virtual Reparacione? IdReparacionesNavigation { get; set; }

    public virtual Servicio? IdserviciosNavigation { get; set; }
}
