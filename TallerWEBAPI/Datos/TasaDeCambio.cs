using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class TasaDeCambio
{
    public int IdTasaCambio { get; set; }

    public DateOnly FechaCambio { get; set; }

    public float ValorCambio { get; set; }
}
