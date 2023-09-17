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
    public class FavoritoesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public FavoritoesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Favoritoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favorito>>> GetFavoritos()
        {
          if (_context.Favoritos == null)
          {
              return NotFound();
          }
            return await _context.Favoritos.ToListAsync();
        }

        // GET: api/Favoritoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Favorito>> GetFavorito(int id)
        {
          if (_context.Favoritos == null)
          {
              return NotFound();
          }
            var favorito = await _context.Favoritos.FindAsync(id);

            if (favorito == null)
            {
                return NotFound();
            }

            return favorito;
        }

        // GET: api/Favoritoes/5
        [HttpGet("IdUsuario/{id}")]
        public async Task<ActionResult<IEnumerable<Favorito>>> GetFavoritoByUserId(int id)
        {
            if (_context.Favoritos == null)
            {
                return NotFound();
            }
            var favoritos = await _context.Favoritos.Where(f => f.IdUsuario == id).ToListAsync();

            if (favoritos == null)
            {
                return NotFound();
            }

            return favoritos;
        }

        // PUT: api/Favoritoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavorito(int id, Favorito favorito)
        {
            if (id != favorito.Id)
            {
                return BadRequest();
            }

            _context.Entry(favorito).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoritoExists(id))
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

        // POST: api/Favoritoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Favorito>> PostFavorito(Favorito favorito)
        {
          if (_context.Favoritos == null)
          {
              return Problem("Entity set 'DatabaseContext.Favoritos'  is null.");
          }
            _context.Favoritos.Add(favorito);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavorito", new { id = favorito.Id }, favorito);
        }

        // DELETE: api/Favoritoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorito(int id)
        {
            if (_context.Favoritos == null)
            {
                return NotFound();
            }
            var favorito = await _context.Favoritos.FindAsync(id);
            if (favorito == null)
            {
                return NotFound();
            }

            _context.Favoritos.Remove(favorito);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Favoritoes/
        [HttpDelete]
        public async Task<IActionResult> DeleteFavorito([FromQuery(Name = "id_usuario")] int id_user, [FromQuery(Name = "id_oferta")] int id_oferta)
        {
            if (_context.Favoritos == null)
            {
                return NotFound();
            }
            var favorito = await _context.Favoritos.Where(f => f.IdOferta == id_oferta).Where(f => f.IdUsuario == id_user).FirstOrDefaultAsync();
            if (favorito == null)
            {
                return NotFound();
            }

            _context.Favoritos.Remove(favorito);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool FavoritoExists(int id)
        {
            return (_context.Favoritos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
