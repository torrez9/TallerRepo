using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string Categoria { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
