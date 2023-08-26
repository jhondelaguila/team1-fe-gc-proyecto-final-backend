using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using team1_fe_gc_proyecto_final_backend.Data;
using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlojamientosController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public AlojamientosController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Alojamientos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alojamiento>>> GetAlojamientos()
        {
          if (_context.Alojamientos == null)
          {
              return NotFound();
          }
            return await _context.Alojamientos.ToListAsync();
        }

        // GET: api/Alojamientos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Alojamiento>> GetAlojamiento(int id)
        {
          if (_context.Alojamientos == null)
          {
              return NotFound();
          }
            var alojamiento = await _context.Alojamientos.FindAsync(id);

            if (alojamiento == null)
            {
                return NotFound();
            }

            return alojamiento;
        }

        // PUT: api/Alojamientos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlojamiento(int id, Alojamiento alojamiento)
        {
            if (id != alojamiento.Id)
            {
                return BadRequest();
            }

            _context.Entry(alojamiento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlojamientoExists(id))
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

        // POST: api/Alojamientos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Alojamiento>> PostAlojamiento(Alojamiento alojamiento)
        {
          if (_context.Alojamientos == null)
          {
              return Problem("Entity set 'DataBaseContext.Alojamientos'  is null.");
          }
            _context.Alojamientos.Add(alojamiento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlojamiento", new { id = alojamiento.Id }, alojamiento);
        }

        // DELETE: api/Alojamientos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlojamiento(int id)
        {
            if (_context.Alojamientos == null)
            {
                return NotFound();
            }
            var alojamiento = await _context.Alojamientos.FindAsync(id);
            if (alojamiento == null)
            {
                return NotFound();
            }

            _context.Alojamientos.Remove(alojamiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlojamientoExists(int id)
        {
            return (_context.Alojamientos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
