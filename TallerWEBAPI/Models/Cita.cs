using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Models;

public partial class Cita
{
    public int IdCita { get; set; }

    public int IdCliente { get; set; }

    public DateOnly FechaCita { get; set; }

    public string? Estado { get; set; }

    public string? Descripcion { get; set; }

    public TimeOnly? Hora { get; set; }
    public virtual Cliente? IdClienteNavigation { get; set; } = null!;
}
