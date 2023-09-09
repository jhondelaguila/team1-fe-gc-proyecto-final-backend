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
    public class OfertasController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public OfertasController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Ofertas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Oferta>>> GetOfertas()
        {
          if (_context.Ofertas == null)
          {
              return NotFound();
          }
            return await _context.Ofertas.ToListAsync();
        }

        // GET: api/Ofertas
        [HttpGet("/api/Ofertas/Filtros")]
        public async Task<ActionResult<IEnumerable<Oferta>>> GetDatosFiltros()
        {
            if (_context.Ofertas == null)
            {
                return NotFound();
            }
            List<Oferta> listOfertas = await _context.Ofertas.ToListAsync();
            List<Alojamiento> listAlojamientos = new List<Alojamiento>();

            for (int i = 0;i<listOfertas.Count; i++)
            {
                var alojamiento = await _context.Alojamientos.FindAsync(listOfertas[i].IdAlojamiento);
                if (alojamiento != null)
                {
                    listAlojamientos.Add(alojamiento);
                }    
            }

            List<ServiciosAlojamientos> listS_A = new List<ServiciosAlojamientos>();

            for (int i = 0; i < listAlojamientos.Count; i++)
            {
                var s_a = await _context.ServiciosAlojamientos.Where(sa => sa.IdAlojamiento == listAlojamientos[i].Id).ToListAsync();
                if (s_a != null)
                {
                    for(int j=0; j < s_a.Count; j++)
                    {
                        listS_A.Add(s_a[i]);
                    }
                }
            }

            return listOfertas;
        }

        // GET: api/Ofertas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Oferta>> GetOferta(int id)
        {
          if (_context.Ofertas == null)
          {
              return NotFound();
          }
            var oferta = await _context.Ofertas.FindAsync(id);

            if (oferta == null)
            {
                return NotFound();
            }

            return oferta;
        }


        [HttpGet("Buscar")]
        public async Task<ActionResult<IEnumerable<Oferta>>> GetBuscarOfertas([FromQuery(Name = "nombre")] string? nombre, [FromQuery(Name = "fecha_inicio")] string? fecha_inicio, [FromQuery(Name = "fecha_fin")] string? fecha_fin, [FromQuery(Name = "num_personas")] int? num_personas)
        {
            if (_context.Ofertas == null)
            {
                return NotFound();
            }

            IQueryable<Oferta> query = _context.Ofertas;

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(p => p.Titulo == nombre);
            }

            string[] date;
            DateOnly date_inicio;
            if (!string.IsNullOrEmpty(fecha_inicio))
            {
                date = fecha_inicio.Split("-");
                date_inicio = new DateOnly(Int32.Parse(date[0]), Int32.Parse(date[1]), Int32.Parse(date[2]));
                query = query.Where(p => p.FechaInicio >= date_inicio);
            }
            else
            {
                return BadRequest();
            }

            string[] date2;
            DateOnly date_fin;
            if (!string.IsNullOrEmpty(fecha_fin) && !fecha_fin.Equals(fecha_inicio))
            {
                date2 = fecha_fin.Split("-");
                date_fin = new DateOnly(Int32.Parse(date2[0]), Int32.Parse(date2[1]), Int32.Parse(date2[2]));
                query = query.Where(p => p.FechaFin <= date_fin);
            }

            if (num_personas != null)
            {
                query = query.Where(p => p.MaxPersonas >= num_personas);
            }

            return await query.ToListAsync();
        }

        // PUT: api/Ofertas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOferta(int id, Oferta oferta)
        {
            if (id != oferta.Id)
            {
                return BadRequest();
            }

            _context.Entry(oferta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfertaExists(id))
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

        // POST: api/Ofertas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Oferta>> PostOferta(Oferta oferta)
        {
          if (_context.Ofertas == null)
          {
              return Problem("Entity set 'DatabaseContext.Ofertas'  is null.");
          }
            _context.Ofertas.Add(oferta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOferta", new { id = oferta.Id }, oferta);
        }

        // DELETE: api/Ofertas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOferta(int id)
        {
            if (_context.Ofertas == null)
            {
                return NotFound();
            }
            var oferta = await _context.Ofertas.FindAsync(id);
            if (oferta == null)
            {
                return NotFound();
            }

            _context.Ofertas.Remove(oferta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OfertaExists(int id)
        {
            return (_context.Ofertas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
