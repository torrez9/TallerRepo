using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class OtrasSalidasDeInventario
{
    public int IdInventario { get; set; }

    public string CodigoProducto { get; set; } = null!;

    public int CantidadSalir { get; set; }

    public string MotivoSalida { get; set; } = null!;

    public string DescripcionSalida { get; set; } = null!;

    public virtual Producto CodigoProductoNavigation { get; set; } = null!;
}
