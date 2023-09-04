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
    public class NivelsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public NivelsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Nivels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nivel>>> GetNiveles()
        {
          if (_context.Niveles == null)
          {
              return NotFound();
          }
            return await _context.Niveles.ToListAsync();
        }

        // GET: api/Nivels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Nivel>> GetNivel(int id)
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

        // PUT: api/Nivels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNivel(int id, Nivel nivel)
        {
            if (id != nivel.Id)
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

        // POST: api/Nivels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Nivel>> PostNivel(Nivel nivel)
        {
          if (_context.Niveles == null)
          {
              return Problem("Entity set 'DatabaseContext.Niveles'  is null.");
          }
            _context.Niveles.Add(nivel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNivel", new { id = nivel.Id }, nivel);
        }

        // DELETE: api/Nivels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNivel(int id)
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

        private bool NivelExists(int id)
        {
            return (_context.Niveles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
