using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taller.Models;

public class Cita
{
    public int IdCita { get; set; }
    public int IdCliente { get; set; }
    public DateTime FechaCita { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public string? Descripcion { get; set; }
    public virtual Cliente? IdClienteNavigation { get; set; }
}