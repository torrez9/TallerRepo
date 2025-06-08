using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class Marca
{
    public int IdMarca { get; set; }

    public string Marca1 { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
