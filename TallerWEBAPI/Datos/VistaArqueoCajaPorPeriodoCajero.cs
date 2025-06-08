using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class VistaArqueoCajaPorPeriodoCajero
{
    public int IdArqueoCaja { get; set; }

    public string Cajero { get; set; } = null!;

    public int IdApertura { get; set; }

    public DateOnly FechaApertura { get; set; }

    public TimeOnly HoraApertura { get; set; }

    public float MontoApertura { get; set; }

    public float TotalEfectivoCordoba { get; set; }

    public float TotalEfectivoDolar { get; set; }

    public float? FaltanteCordoba { get; set; }

    public float? FaltanteDolar { get; set; }

    public float? SobranteCordoba { get; set; }

    public float? SobranteDolar { get; set; }

    public DateTime FechaArqueo { get; set; }

    public float TotalVentasCordobas { get; set; }

    public float TotalVentasDolares { get; set; }
}
