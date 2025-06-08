using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class DetalleDeVentum
{
    public int IdDetalleVenta { get; set; }

    public int IdVenta { get; set; }

    public string CodigoProducto { get; set; } = null!;

    public int Cantidad { get; set; }

    public string? Precio { get; set; }

    public string CedulaCliente { get; set; } = null!;

    public double? SubTotal { get; set; }

    public virtual Producto CodigoProductoNavigation { get; set; } = null!;

    public virtual Ventum IdVentaNavigation { get; set; } = null!;
}
