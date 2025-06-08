using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class FacturaCredito
{
    public int IdCredito { get; set; }

    public int IdVenta { get; set; }

    public string EstadoCredito { get; set; } = null!;

    public float TotalAbonado { get; set; }

    public float NuevoSaldo { get; set; }

    public string? Observaciones { get; set; }

    public int PlazosMeses { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFinal { get; set; }

    public float MontoCredito { get; set; }

    public float InteresMensual { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<DetalleCredito> DetalleCreditos { get; set; } = new List<DetalleCredito>();

    public virtual Ventum IdVentaNavigation { get; set; } = null!;
}
