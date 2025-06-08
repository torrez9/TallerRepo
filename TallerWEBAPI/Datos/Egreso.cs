using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class Egreso
{
    public int IdEgreso { get; set; }

    public int IdArqueoCaja { get; set; }

    public int IdUsuario { get; set; }

    public int IdApertura { get; set; }

    public DateOnly FechaEgreso { get; set; }

    public decimal? CantidadEgresadaCordoba { get; set; }

    public string MotivoEgreso { get; set; } = null!;

    public decimal? CantidadEgresadaDolar { get; set; }

    public virtual ArqueoCaja ArqueoCaja { get; set; } = null!;
}
