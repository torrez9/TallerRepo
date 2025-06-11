using System;
using System.Collections.Generic;

// Models/Cliente.cs
namespace TallerWEBAPI.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Apellido { get; set; }
    public string Telefono { get; set; } = null!;
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
    public string? Usuario { get; set; }
    public string? Contraseña { get; set; }
    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
    public virtual ICollection<Moto> Motos { get; set; } = new List<Moto>();
}

public class RegistroResultado
{
    public bool Exito { get; set; }
    public string? MensajeError { get; set; }
}

public class ResetPasswordResult
{
    public bool Exito { get; set; }
    public string? MensajeError { get; set; }
}