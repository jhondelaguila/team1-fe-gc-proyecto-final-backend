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
    public class OfertasImagenesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public OfertasImagenesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/OfertasImagenes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfertasImagenes>>> GetOfertasImagenes()
        {
          if (_context.OfertasImagenes == null)
          {
              return NotFound();
          }
            return await _context.OfertasImagenes.ToListAsync();
        }

        // GET: api/OfertasImagenes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfertasImagenes>> GetOfertasImagenes(int id)
        {
          if (_context.OfertasImagenes == null)
          {
              return NotFound();
          }
            var ofertasImagenes = await _context.OfertasImagenes.FindAsync(id);

            if (ofertasImagenes == null)
            {
                return NotFound();
            }

            return ofertasImagenes;
        }

        // PUT: api/OfertasImagenes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfertasImagenes(int id, OfertasImagenes ofertasImagenes)
        {
            if (id != ofertasImagenes.Id)
            {
                return BadRequest();
            }

            _context.Entry(ofertasImagenes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfertasImagenesExists(id))
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

        // POST: api/OfertasImagenes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OfertasImagenes>> PostOfertasImagenes(OfertasImagenes ofertasImagenes)
        {
          if (_context.OfertasImagenes == null)
          {
              return Problem("Entity set 'DatabaseContext.OfertasImagenes'  is null.");
          }
            _context.OfertasImagenes.Add(ofertasImagenes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOfertasImagenes", new { id = ofertasImagenes.Id }, ofertasImagenes);
        }

        // DELETE: api/OfertasImagenes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfertasImagenes(int id)
        {
            if (_context.OfertasImagenes == null)
            {
                return NotFound();
            }
            var ofertasImagenes = await _context.OfertasImagenes.FindAsync(id);
            if (ofertasImagenes == null)
            {
                return NotFound();
            }

            _context.OfertasImagenes.Remove(ofertasImagenes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OfertasImagenesExists(int id)
        {
            return (_context.OfertasImagenes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
