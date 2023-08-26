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
    public class NivelesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public NivelesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Niveles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nivel>>> GetNiveles()
        {
          if (_context.Niveles == null)
          {
              return NotFound();
          }
            return await _context.Niveles.ToListAsync();
        }

        // GET: api/Niveles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Nivel>> GetNivel(string id)
        {
          if (_context.Niveles == null)
          {
              return NotFound();
          }
            var nivel = await _context.Niveles.FindAsync(id);

            if (nivel == null)
            {
                return NotFound();
            }

            return nivel;
        }

        // PUT: api/Niveles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNivel(string id, Nivel nivel)
        {
            if (id != nivel.Nombre)
            {
                return BadRequest();
            }

            _context.Entry(nivel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NivelExists(id))
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

        // POST: api/Niveles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Nivel>> PostNivel(Nivel nivel)
        {
          if (_context.Niveles == null)
          {
              return Problem("Entity set 'DataBaseContext.Niveles'  is null.");
          }
            _context.Niveles.Add(nivel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NivelExists(nivel.Nombre))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNivel", new { id = nivel.Nombre }, nivel);
        }

        // DELETE: api/Niveles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNivel(string id)
        {
            if (_context.Niveles == null)
            {
                return NotFound();
            }
            var nivel = await _context.Niveles.FindAsync(id);
            if (nivel == null)
            {
                return NotFound();
            }

            _context.Niveles.Remove(nivel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NivelExists(string id)
        {
            return (_context.Niveles?.Any(e => e.Nombre == id)).GetValueOrDefault();
        }
    }
}
