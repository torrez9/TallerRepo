using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class Cliente
{
    public string CedulaCliente { get; set; } = null!;

    public string NombreCliente { get; set; } = null!;

    public string ApellidoCliente { get; set; } = null!;

    public bool? SujetoCredito { get; set; }

    public string TelefonoCliente { get; set; } = null!;

    public string? ColillaInssCliente { get; set; }

    public string? DireccionCliente { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
