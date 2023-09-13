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
    public class OfertasActividadesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public OfertasActividadesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/OfertasActividades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfertasActividades>>> GetOfertasActividades()
        {
          if (_context.OfertasActividades == null)
          {
              return NotFound();
          }
            return await _context.OfertasActividades.ToListAsync();
        }

        // GET: api/OfertasActividades/5
        [HttpGet("IdOferta/{id}")]
        public async Task<ActionResult<IEnumerable<OfertasActividades>>> GetOfertasActividadesByOfertaId(int id)
        {
            if (_context.OfertasActividades == null)
            {
                return NotFound();
            }
            var ofertasActividades = await _context.OfertasActividades.Where(O_A => O_A.IdOferta == id).ToListAsync();

            if (ofertasActividades == null)
            {
                return NotFound();
            }

            return ofertasActividades;
        }

        // GET: api/OfertasActividades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfertasActividades>> GetOfertasActividades(int id)
        {
          if (_context.OfertasActividades == null)
          {
              return NotFound();
          }
            var ofertasActividades = await _context.OfertasActividades.FindAsync(id);

            if (ofertasActividades == null)
            {
                return NotFound();
            }

            return ofertasActividades;
        }

        // PUT: api/OfertasActividades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfertasActividades(int id, OfertasActividades ofertasActividades)
        {
            if (id != ofertasActividades.Id)
            {
                return BadRequest();
            }

            _context.Entry(ofertasActividades).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfertasActividadesExists(id))
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

        // POST: api/OfertasActividades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OfertasActividades>> PostOfertasActividades(OfertasActividades ofertasActividades)
        {
          if (_context.OfertasActividades == null)
          {
              return Problem("Entity set 'DatabaseContext.OfertasActividades'  is null.");
          }
            _context.OfertasActividades.Add(ofertasActividades);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOfertasActividades", new { id = ofertasActividades.Id }, ofertasActividades);
        }

        // DELETE: api/OfertasActividades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfertasActividades(int id)
        {
            if (_context.OfertasActividades == null)
            {
                return NotFound();
            }
            var ofertasActividades = await _context.OfertasActividades.FindAsync(id);
            if (ofertasActividades == null)
            {
                return NotFound();
            }

            _context.OfertasActividades.Remove(ofertasActividades);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OfertasActividadesExists(int id)
        {
            return (_context.OfertasActividades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
