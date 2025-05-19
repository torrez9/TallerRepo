using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? Correo { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Moto> Motos { get; set; } = new List<Moto>();

    public virtual ICollection<Moto> IdMotos { get; set; } = new List<Moto>();
}
