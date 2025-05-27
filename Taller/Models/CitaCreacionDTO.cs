namespace Taller.Models
{
    public class CitaCreacionDTO
    {
        public int IdCliente { get; set; }
        public DateTime FechaCita { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string? Descripcion { get; set; }
    }
}
