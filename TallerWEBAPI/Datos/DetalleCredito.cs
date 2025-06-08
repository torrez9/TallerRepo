using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class DetalleCredito
{
    public int IdDetalleCredito { get; set; }

    public int IdCredito { get; set; }

    public int NumeroCuota { get; set; }

    public DateTime FechaPago { get; set; }

    public float ValorCuota { get; set; }

    public float AbonoCapital { get; set; }

    public float InteresPagado { get; set; }

    public float TotalCordobas { get; set; }

    public float TotalDolares { get; set; }

    public float? CambioDevuelto { get; set; }

    public string? Observaciones { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public virtual FacturaCredito IdCreditoNavigation { get; set; } = null!;
}
