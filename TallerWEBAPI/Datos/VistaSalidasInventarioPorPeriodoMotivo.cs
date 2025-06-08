using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class VistaSalidasInventarioPorPeriodoMotivo
{
    public int IdInventario { get; set; }

    public string CodigoProducto { get; set; } = null!;

    public string Producto { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public int CantidadSalir { get; set; }

    public string MotivoSalida { get; set; } = null!;

    public string DescripcionSalida { get; set; } = null!;

    public DateTime? FechaSalida { get; set; }
}
