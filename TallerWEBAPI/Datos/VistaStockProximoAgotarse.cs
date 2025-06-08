using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class VistaStockProximoAgotarse
{
    public string CodigoProducto { get; set; } = null!;

    public string Producto { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public float PrecioVenta { get; set; }

    public int? StockActual { get; set; }

    public int? StockMinimo { get; set; }

    public int? Diferencia { get; set; }

    public string EstadoReabastecimiento { get; set; } = null!;
}
