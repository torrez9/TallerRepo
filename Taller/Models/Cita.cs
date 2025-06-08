using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taller.Models;

public partial class Cita
{
    public int IdCita { get; set; }

    public int IdCliente { get; set; }

    public DateOnly FechaCita { get; set; }

    public string? Estado { get; set; }

    public string? Descripcion { get; set; }

    public TimeOnly? Hora { get; set; }
    public bool Notificada { get; set; } // Nueva propiedad


    public virtual Cliente? IdClienteNavigation { get; set; } = null!;
}