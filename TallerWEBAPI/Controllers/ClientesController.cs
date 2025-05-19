using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // Inyecta ambos servicios en el constructor
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
                    // Eliminar citas asociadas
                    await _citaService.EliminarCitasPorClienteAsync(id);
                    // Eliminar facturas asociadas (si existe el servicio)
                    // await _facturaService.EliminarFacturasPorClienteAsync(id);
                    // Eliminar motos asociadas (si existe el servicio)
                    // await _motoService.EliminarMotosPorClienteAsync(id);
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
}