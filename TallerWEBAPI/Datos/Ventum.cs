using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class Ventum
{
    public int IdVenta { get; set; }

    public string CedulaCliente { get; set; } = null!;

    public DateOnly FechaVenta { get; set; }

    public string TipoPago { get; set; } = null!;

    public float? PagoCordobas { get; set; }

    public float? PagoDolares { get; set; }

    public float? CambioVenta { get; set; }

    public float TotalVenta { get; set; }

    public virtual Cliente CedulaClienteNavigation { get; set; } = null!;

    public virtual ICollection<DetalleDeVentum> DetalleDeVenta { get; set; } = new List<DetalleDeVentum>();

    public virtual ICollection<Devolucion> Devolucions { get; set; } = new List<Devolucion>();

    public virtual ICollection<FacturaCredito> FacturaCreditos { get; set; } = new List<FacturaCredito>();
}
