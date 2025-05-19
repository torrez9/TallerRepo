using System;
using System.Collections.Generic;

namespace Taller.Models;

public partial class Moto
{
    public int IdMoto { get; set; }

    public int? IdCliente { get; set; }

    public string Placa { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public string Modelo { get; set; } = null!;

    public int Año { get; set; }

    public string Color { get; set; } = null!;

    public string EstadoEntrada { get; set; } = null!;

    public string? EstadoSalida { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual ICollection<Reparacione> Reparaciones { get; set; } = new List<Reparacione>();

    public virtual ICollection<Cliente> IdClientes { get; set; } = new List<Cliente>();
}
