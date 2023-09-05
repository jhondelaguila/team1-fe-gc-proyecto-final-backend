using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using team1_fe_gc_proyecto_final_backend.Data;
using team1_fe_gc_proyecto_final_backend.Interfaces;
using team1_fe_gc_proyecto_final_backend.Models;
using Newtonsoft.Json.Linq;

namespace team1_fe_gc_proyecto_final_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ActividadsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Actividads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actividad>>> GetActividades()
        {
          if (_context.Actividades == null)
          {
              return NotFound();
          }
            return await _context.Actividades.ToListAsync();
        }

        // GET: api/Actividads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actividad>> GetActividad(int id)
        {
          if (_context.Actividades == null)
          {
              return NotFound();
          }
            var actividad = await _context.Actividades.FindAsync(id);

            if (actividad == null)
            {
                return NotFound();
            }

            return actividad;
        }

        // PUT: api/Actividads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActividad(int id, Actividad actividad)
        {
            if (id != actividad.Id)
            {
                return BadRequest();
            }

            _context.Entry(actividad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActividadExists(id))
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

        // POST: api/Actividads
        [HttpPost]
        public async Task<ActionResult<IActividadCrear>> PostActividad([FromBody] ActividadCrear actividadObject)
        {
            
 

            if (actividadObject == null)
            {
                return BadRequest("Error en el json");
            }

            if (_context.Direcciones == null)
            {
                return Problem("Entity set 'DatabaseContext.Direcciones'  is null.");
            }

            //Obtengo y añado la direccion del objeto que me llega del front
            Direccion direccion = new Direccion(actividadObject.Pais, actividadObject.Provincia, actividadObject.Localidad, actividadObject.CodigoPostal, actividadObject.Calle, actividadObject.Numero);
            _context.Direcciones.Add(direccion);

            //hago efectivo el Add en la base de datos para poder accedder al id
            await _context.SaveChangesAsync();

            int idDireccion = await _context.Direcciones
                .Where(d => d.Pais == actividadObject.Pais && d.Provincia == actividadObject.Provincia && d.Localidad == actividadObject.Localidad && d.CodigoPostal == actividadObject.CodigoPostal && d.Calle == actividadObject.Calle && d.Numero == actividadObject.Numero)
                .Select(d => d.Id)
                .FirstOrDefaultAsync();

            //Obtengo la actividad del objjeto que me llega del front
            Actividad actividad = new Actividad(actividadObject.Titulo, actividadObject.Descripcion, idDireccion);
            _context.Actividades.Add(actividad);
            await _context.SaveChangesAsync();

            //obtengo el id de la actividad que acabo de añadir para crear todas las imagenes (idActividad es un atributo -FK- necesario para crear la imagenes de actividades)
            int IdActividad = await _context.Actividades
                .Where(a => a.Titulo == actividadObject.Titulo && a.Descripcion == actividadObject.Descripcion && a.IdDireccion == idDireccion)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            //añado todas las imagenes de la actividad
            foreach (string url in actividadObject.Imagenes){
                Imagen imagen = new Imagen(url, IdActividad);
                _context.Imagenes.Add(imagen);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActividad", new { id = actividad.Id }, actividad);
        }

        // DELETE: api/Actividads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActividad(int id)
        {
            if (_context.Actividades == null)
            {
                return NotFound();
            }
            var actividad = await _context.Actividades.FindAsync(id);
            if (actividad == null)
            {
                return NotFound();
            }

            _context.Actividades.Remove(actividad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActividadExists(int id)
        {
            return (_context.Actividades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
