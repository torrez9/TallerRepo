using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Models;

public partial class DetalleReparacion
{
    public int IdDetalleRep { get; set; }

    public int? IdReparaciones { get; set; }

    public int? Idservicios { get; set; }

    public decimal? CostoServicio { get; set; }

    public virtual Reparacione? IdReparacionesNavigation { get; set; }

    public virtual Servicio? IdserviciosNavigation { get; set; }
}
