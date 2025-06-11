using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TallerWEBAPI.Models;
using TallerWEBAPI.Services;

namespace TallerWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly MotosTuningContext _context;
        private readonly CitaService _citaService;

        public ClientesController(MotosTuningContext context, CitaService citaService)
        {
            _context = context;
            _citaService = citaService ?? throw new ArgumentNullException(nameof(citaService));
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            try
            {
                var clientes = await _context.Clientes.ToListAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener los clientes.",
                    detalles = ex.Message
                });
            }
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    return NotFound($"No se encontró el cliente con ID {id}");
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener el cliente.",
                    detalles = ex.Message
                });
            }
        }

        // GET: api/Clientes/PorUsuario/{usuario}
        [HttpGet("PorUsuario/{usuario}")]
        public async Task<ActionResult<Cliente>> GetClientePorUsuario(string usuario)
        {
            try
            {
                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.Usuario == usuario);

                if (cliente == null)
                {
                    return NotFound($"No se encontró el cliente con usuario {usuario}");
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener el cliente.",
                    detalles = ex.Message
                });
            }
        }

        // GET: api/Clientes/VerificarRelaciones/5
        [HttpGet("VerificarRelaciones/{id}")]
        public async Task<ActionResult<bool>> VerificarRelaciones(int id)
        {
            try
            {
                var tieneCitas = await _context.Citas.AnyAsync(c => c.IdCliente == id);
                var tieneFacturas = await _context.Facturas.AnyAsync(f => f.IdCliente == id);
                var tieneMotos = await _context.Motos.AnyAsync(m => m.IdCliente == id);

                return Ok(tieneCitas || tieneFacturas || tieneMotos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al verificar relaciones.",
                    detalles = ex.Message
                });
            }
        }

        // POST: api/Clientes
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] Cliente cliente)
        {
            try
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al crear el cliente.",
                    detalles = ex.Message
                });
            }
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return BadRequest("El ID del cliente no coincide con el de la URL.");
            }

            try
            {
                var clienteExistente = await _context.Clientes.FindAsync(id);
                if (clienteExistente == null)
                {
                    return NotFound($"No se encontró el cliente con ID {id}");
                }

                clienteExistente.Nombre = cliente.Nombre;
                clienteExistente.Telefono = cliente.Telefono;
                clienteExistente.Correo = cliente.Correo;
                clienteExistente.Direccion = cliente.Direccion;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al actualizar el cliente.",
                    detalles = ex.Message
                });
            }
        }

        // PUT: api/Clientes/Perfil/5
        [HttpPut("Perfil/{id}")]
        public async Task<IActionResult> PutPerfilCliente(int id, [FromBody] Cliente cliente)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId != id.ToString())
            {
                return Forbid();
            }

            try
            {
                var clienteExistente = await _context.Clientes.FindAsync(id);
                if (clienteExistente == null)
                {
                    return NotFound($"No se encontró el cliente con ID {id}");
                }

                clienteExistente.Nombre = cliente.Nombre;
                clienteExistente.Apellido = cliente.Apellido;
                clienteExistente.Telefono = cliente.Telefono;
                clienteExistente.Direccion = cliente.Direccion;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al actualizar el perfil.",
                    detalles = ex.Message
                });
            }
        }

        // POST: api/Clientes/CambiarContraseña
        [HttpPost("CambiarContraseña")]
        public async Task<IActionResult> CambiarContraseña([FromBody] CambioContraseñaModel model)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized();

            var cliente = await _context.Clientes.FindAsync(int.Parse(usuarioId));
            if (cliente == null)
                return NotFound();

            if (cliente.Contraseña != model.ContraseñaActual)
            {
                return BadRequest("La contraseña actual no es correcta");
            }

            if (model.NuevaContraseña != model.ConfirmarContraseña)
            {
                return BadRequest("Las contraseñas nuevas no coinciden");
            }

            cliente.Contraseña = model.NuevaContraseña;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id, [FromQuery] bool eliminarRelaciones = false)
        {
            try
            {
                var cliente = await _context.Clientes
                    .Include(c => c.Cita)
                    .Include(c => c.Facturas)
                    .Include(c => c.Motos)
                    .FirstOrDefaultAsync(c => c.IdCliente == id);

                if (cliente == null)
                {
                    return NotFound($"No se encontró el cliente con ID {id}");
                }

                if (eliminarRelaciones)
                {
                    await _citaService.EliminarCitasPorClienteAsync(id);
                }
                else if (cliente.Cita.Any() || cliente.Facturas.Any() || cliente.Motos.Any())
                {
                    return BadRequest(new
                    {
                        mensaje = "El cliente tiene registros asociados.",
                        detalles = "Pase 'eliminarRelaciones=true' como parámetro para eliminarlos también."
                    });
                }

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al eliminar el cliente.",
                    detalles = ex.Message
                });
            }
        }
    }

    public class CambioContraseñaModel
    {
        public string ContraseñaActual { get; set; }
        public string NuevaContraseña { get; set; }
        public string ConfirmarContraseña { get; set; }
    }
}