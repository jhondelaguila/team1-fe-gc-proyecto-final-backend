using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using team1_fe_gc_proyecto_final_backend.Data;
using team1_fe_gc_proyecto_final_backend.Models;
using team1_fe_gc_proyecto_final_backend.DTOs;
using Google.Protobuf.Collections;

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
        public async Task<ActionResult<FiltrosResponseDto>> GetDatosFiltros()
        {
            if (_context.Ofertas == null)
            {
                return NotFound();
            }
            List<Oferta> listOfertas = await _context.Ofertas.ToListAsync();
            List<Alojamiento> listAlojamientos = await _context.Alojamientos.ToListAsync();
            List<ServiciosAlojamientos> listS_A = await _context.ServiciosAlojamientos.ToListAsync();

            var response = new FiltrosResponseDto
            {
                Ofertas = listOfertas,
                Alojamientos = listAlojamientos,
                S_A = listS_A

            };


            return response;
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
        public async Task<ActionResult<FiltrosResponseDto>> GetBuscarOfertas([FromQuery(Name = "nombre")] string? nombre, [FromQuery(Name = "fecha_inicio")] string? fecha_inicio, [FromQuery(Name = "fecha_fin")] string? fecha_fin, [FromQuery(Name = "num_personas")] int? num_personas)
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

            List<Oferta> listOfertas = await query.ToListAsync();
            List<Alojamiento> listAlojamientos = new List<Alojamiento>();
            if (listOfertas != null)
            {
                for (int i = 0; i < listOfertas.Count; i++)
                {
                    var alojamiento = await _context.Alojamientos.FindAsync(listOfertas[i].IdAlojamiento);
                    if (alojamiento != null && !listAlojamientos.Contains(alojamiento))
                    {
                        listAlojamientos.Add(alojamiento);
                    }
                }
            }

            List<ServiciosAlojamientos> s_a = new List<ServiciosAlojamientos>();

            for (int i = 0; i < listAlojamientos.Count; i++)
            {
                var listS_A = await _context.ServiciosAlojamientos.Where(sa => sa.IdAlojamiento == listAlojamientos[i].Id).ToListAsync();
                if (listS_A != null)
                {
                    for (int j = 0; j < listS_A.Count; j++)
                    {
                        s_a.Add(listS_A[j]);
                    }
                }
            }

            var response = new FiltrosResponseDto
            {
                Ofertas = listOfertas,
                Alojamientos = listAlojamientos,
                S_A = s_a
            };

            return response;
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