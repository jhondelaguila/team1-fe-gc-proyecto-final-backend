using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using team1_fe_gc_proyecto_final_backend.Data;
using team1_fe_gc_proyecto_final_backend.Interfaces;
using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ReservasController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Reservas/usuario/6
        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<ReservasOfertas>>> GetReservas(int idUsuario)
        {
          if (_context.Reservas == null)
          {
              return NotFound();
          }
            try
            {
                var reservasUsuario = await _context.Reservas
                    .Where(r => r.IdUsuario == idUsuario)
                    .Join(_context.Ofertas, r => r.IdOferta, o => o.Id, (reserva, oferta) => new ReservasOfertas
                    {
                        IdReserva = reserva.Id,
                        TituloOferta = oferta.Titulo,
                        FechaIni = reserva.FechaInicio,
                        FechaFinal = reserva.FechaFin,
                        Direccion = _context.Alojamientos
                            .Where(a => a.Id == oferta.IdAlojamiento)
                            .Select(a => _context.Direcciones.FirstOrDefault(d => d.Id == a.IdDireccion))
                            .FirstOrDefault(),
                        Estado = reserva.Estado,
                        PrecioOferta = oferta.Precio,
                        ImagenOferta = _context.OfertasImagenes
                            .Where(oi => oi.IdOferta == oferta.Id)
                            .Join(_context.Imagenes, oi => oi.IdImagen, i => i.Id, (oi, i) => i.Url)
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                return Ok(reservasUsuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetListReservas()
        {
            if (_context.Reservas == null)
            {
                return NotFound();
            }
            return await _context.Reservas.ToListAsync();
        }

        // GET: api/Reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
          if (_context.Reservas == null)
          {
              return NotFound();
          }
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return reserva;
        }

        // PUT: api/Reservas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return BadRequest();
            }

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reservas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
          if (_context.Reservas == null)
          {
              return Problem("Entity set 'DatabaseContext.Reservas'  is null.");
          }
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReserva", new { id = reserva.Id }, reserva);
        }

        // DELETE: api/Reservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            if (_context.Reservas == null)
            {
                return NotFound();
            }
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservaExists(int id)
        {
            return (_context.Reservas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
