﻿using System;
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
        public async Task<ActionResult<IEnumerable<ActividadCard>>> GetActividades()
        {
          if (_context.Actividades == null)
          {
              return NotFound();
          }
        

           var actividadCard = await _context.Actividades
               .Select(a => new ActividadCard
               {
                   Id = a.Id,
                   Titulo = a.Titulo,
                   Descripcion = a.Descripcion,
                   Pais = _context.Direcciones.Where(d => d.Id == a.IdDireccion).Select(d => d.Pais).FirstOrDefault(),
                   Localidad = _context.Direcciones.Where(d => d.Id == a.IdDireccion).Select(d => d.Localidad).FirstOrDefault(),
                   FotoPortada = _context.Imagenes.Where(i => i.IdActividad == a.Id).Select(i => i.Url).FirstOrDefault()
               })
               .ToListAsync();

           return actividadCard;


        }

        //GET: api/Actividads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActividadCompleta>> GetActividad(int id)
        {
            var actividad = await _context.Actividades.FindAsync(id);

            if (actividad == null)
            {
                return NotFound();
            }

            var actividadCompleta = await _context.Actividades
                .Where(a => a.Id == id)
                .Select(a => new ActividadCompleta
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Descripcion = a.Descripcion,
                    Direccion = _context.Direcciones.FirstOrDefault(d => d.Id == a.IdDireccion),
                    Imagenes = _context.Imagenes
                        .Where(i => i.IdActividad == a.Id)
                        .ToList(),
                })
                .FirstOrDefaultAsync();

            return actividadCompleta;
        }

        // PUT: api/Actividads/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActividad(int id, ActividadCompleta actividadCompleta)
        {
            if (id != actividadCompleta.Id)
            {
                throw new Exception($"{id} no es igual a {actividadCompleta.Id}");
            }

            // obtenemos la direccion del objeto completo
            Direccion direccion = actividadCompleta.Direccion;

            _context.Entry(direccion).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            var sqlQueryImagenes = $"SELECT * FROM imagenes WHERE id_actividad = {id}";
            var imagenes = await _context.Imagenes.FromSqlRaw(sqlQueryImagenes).ToListAsync();

            foreach (var imagenExistente in actividadCompleta.Imagenes.ToList())
            {
                if (!actividadCompleta.Imagenes.Any(i => i.Id == imagenExistente.Id)) // si las imagenes de la db no existen en la lista de imagenes de la actividad a actualizar, significa que el usuario las ha borrado, y por eso las borramos
                {
                    _context.Imagenes.Remove(imagenExistente);
                }
            }

            foreach (var imagenNueva in actividadCompleta.Imagenes)
            {
                if (imagenNueva.Id == 0) // si el id es 0, significa que aun no está añadida a la db, y por eso es 0 de default
                {
                    _context.Imagenes.Add(imagenNueva);
                }
            }

            await _context.SaveChangesAsync();

            Actividad actividad = new Actividad
            {
                Id = actividadCompleta.Id,
                Titulo = actividadCompleta.Titulo,
                Descripcion = actividadCompleta.Descripcion,
                IdDireccion = actividadCompleta.Direccion.Id,
            };

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
        public async Task<ActionResult<ActividadCrear>> PostActividad(ActividadCrear actividadObject)
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

            if (_context.Actividades == null)
            {
                return Problem("Entity set 'DatabaseContext.Direcciones'  is null.");
            }

            //Obtengo la actividad del objjeto que me llega del front
            Actividad actividad = new Actividad(actividadObject.Titulo, actividadObject.Descripcion, idDireccion);
            _context.Actividades.Add(actividad);
            await _context.SaveChangesAsync();

            //obtengo el id de la actividad que acabo de añadir para crear todas las imagenes (idActividad es un atributo -FK- necesario para crear la imagenes de actividades)
            int IdActividad = await _context.Actividades
                .Where(a => a.Titulo == actividadObject.Titulo && a.Descripcion == actividadObject.Descripcion && a.IdDireccion == idDireccion)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            if (_context.Imagenes == null)
            {
                return Problem("Entity set 'DatabaseContext.Imagenes'  is null.");
            }
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
