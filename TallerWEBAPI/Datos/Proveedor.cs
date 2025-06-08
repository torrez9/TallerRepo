using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class Proveedor
{
    public string CodigoRuc { get; set; } = null!;

    public string NombreProveedor { get; set; } = null!;

    public string ApellidoProveedor { get; set; } = null!;

    public string TelefonoProveedor { get; set; } = null!;

    public string? CorreoProveedor { get; set; }

    public bool EstadoProveedor { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
