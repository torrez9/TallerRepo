using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class Producto
{
    public string CodigoProducto { get; set; } = null!;

    public int IdCategoria { get; set; }

    public int IdMarca { get; set; }

    public string ModeloProducto { get; set; } = null!;

    public float PrecioVenta { get; set; }

    public bool EstadoProducto { get; set; }

    public int? StockMinimo { get; set; }

    public int? StockActual { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleDeVentum> DetalleDeVenta { get; set; } = new List<DetalleDeVentum>();

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual ICollection<OtrasSalidasDeInventario> OtrasSalidasDeInventarios { get; set; } = new List<OtrasSalidasDeInventario>();
}
