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
    public class ImagensController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ImagensController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Imagens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Imagen>>> GetImagenes()
        {
          if (_context.Imagenes == null)
          {
              return NotFound();
          }
            return await _context.Imagenes.ToListAsync();
        }

        // GET: api/Imagens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Imagen>> GetImagen(int id)
        {
          if (_context.Imagenes == null)
          {
              return NotFound();
          }
            var imagen = await _context.Imagenes.FindAsync(id);

            if (imagen == null)
            {
                return NotFound();
            }

            return imagen;
        }

        // PUT: api/Imagens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImagen(int id, Imagen imagen)
        {
            if (id != imagen.Id)
            {
                return BadRequest();
            }

            _context.Entry(imagen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagenExists(id))
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

        // POST: api/Imagens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Imagen>> PostImagen(Imagen imagen)
        {
          if (_context.Imagenes == null)
          {
              return Problem("Entity set 'DatabaseContext.Imagenes'  is null.");
          }
            _context.Imagenes.Add(imagen);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImagen", new { id = imagen.Id }, imagen);
        }

        // DELETE: api/Imagens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagen(int id)
        {
            if (_context.Imagenes == null)
            {
                return NotFound();
            }
            var imagen = await _context.Imagenes.FindAsync(id);
            if (imagen == null)
            {
                return NotFound();
            }

            _context.Imagenes.Remove(imagen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImagenExists(int id)
        {
            return (_context.Imagenes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
