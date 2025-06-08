using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class AperturaCaja
{
    public int IdApertura { get; set; }

    public float MontoApertura { get; set; }

    public DateOnly FechaApertura { get; set; }

    public TimeOnly HoraApertura { get; set; }

    public string EstadoApertura { get; set; } = null!;

    public virtual ICollection<ArqueoCaja> ArqueoCajas { get; set; } = new List<ArqueoCaja>();
}
