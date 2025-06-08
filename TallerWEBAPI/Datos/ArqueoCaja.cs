using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class ArqueoCaja
{
    public int IdArqueoCaja { get; set; }

    public int IdUsuario { get; set; }

    public int IdApertura { get; set; }

    public float TotalEfectivoCordoba { get; set; }

    public float TotalEfectivoDolar { get; set; }

    public float? FaltanteCordoba { get; set; }

    public float? FaltanteDolar { get; set; }

    public float? SobranteCordoba { get; set; }

    public float? SobranteDolar { get; set; }

    public DateTime FechaArqueo { get; set; }

    public virtual ICollection<Egreso> Egresos { get; set; } = new List<Egreso>();

    public virtual AperturaCaja IdAperturaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
