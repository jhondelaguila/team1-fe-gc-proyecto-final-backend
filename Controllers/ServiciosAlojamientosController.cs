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
    public class ServiciosAlojamientosController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ServiciosAlojamientosController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ServiciosAlojamientos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiciosAlojamientos>>> GetServiciosAlojamientos()
        {
          if (_context.ServiciosAlojamientos == null)
          {
              return NotFound();
          }
            return await _context.ServiciosAlojamientos.ToListAsync();
        }

        // GET: api/ServiciosAlojamientos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiciosAlojamientos>> GetServiciosAlojamientos(int id)
        {
          if (_context.ServiciosAlojamientos == null)
          {
              return NotFound();
          }
            var serviciosAlojamientos = await _context.ServiciosAlojamientos.FindAsync(id);

            if (serviciosAlojamientos == null)
            {
                return NotFound();
            }

            return serviciosAlojamientos;
        }

        // PUT: api/ServiciosAlojamientos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiciosAlojamientos(int id, ServiciosAlojamientos serviciosAlojamientos)
        {
            if (id != serviciosAlojamientos.Id)
            {
                return BadRequest();
            }

            _context.Entry(serviciosAlojamientos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiciosAlojamientosExists(id))
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

        // POST: api/ServiciosAlojamientos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiciosAlojamientos>> PostServiciosAlojamientos(ServiciosAlojamientos serviciosAlojamientos)
        {
          if (_context.ServiciosAlojamientos == null)
          {
              return Problem("Entity set 'DatabaseContext.ServiciosAlojamientos'  is null.");
          }
            _context.ServiciosAlojamientos.Add(serviciosAlojamientos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiciosAlojamientos", new { id = serviciosAlojamientos.Id }, serviciosAlojamientos);
        }

        // DELETE: api/ServiciosAlojamientos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiciosAlojamientos(int id)
        {
            if (_context.ServiciosAlojamientos == null)
            {
                return NotFound();
            }
            var serviciosAlojamientos = await _context.ServiciosAlojamientos.FindAsync(id);
            if (serviciosAlojamientos == null)
            {
                return NotFound();
            }

            _context.ServiciosAlojamientos.Remove(serviciosAlojamientos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiciosAlojamientosExists(int id)
        {
            return (_context.ServiciosAlojamientos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
