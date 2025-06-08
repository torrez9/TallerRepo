using System;
using System.Collections.Generic;

namespace TallerWEBAPI.Datos;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ApellidoUsuario { get; set; } = null!;

    public string ContraseñaUsuario { get; set; } = null!;

    public string CorreoUsuario { get; set; } = null!;

    public string TelefonoUsuario { get; set; } = null!;

    public string RolUsuario { get; set; } = null!;

    public bool EstadoUsuario { get; set; }

    public string UsuarioLogueo { get; set; } = null!;

    public string? TokenRecuperacion { get; set; }

    public DateOnly? FechaRecuperacion { get; set; }

    public virtual ICollection<ArqueoCaja> ArqueoCajas { get; set; } = new List<ArqueoCaja>();
}
