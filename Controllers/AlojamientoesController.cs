using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using team1_fe_gc_proyecto_final_backend.Data;
using team1_fe_gc_proyecto_final_backend.Models;
using team1_fe_gc_proyecto_final_backend.Interfaces;

namespace team1_fe_gc_proyecto_final_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlojamientoesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AlojamientoesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Alojamientoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlojamientoCompleto>>> GetAlojamientos()
        {
            if (_context.Alojamientos == null)
            {
                return NotFound();
            }

            var alojamientos = await _context.Alojamientos.ToListAsync();
            var alojamientosCompletos = new List<AlojamientoCompleto>();

            foreach (var alojamiento in alojamientos)
            {
                var sqlQueryDireccion = $"SELECT * FROM direcciones WHERE id = {alojamiento.IdDireccion}";
                var direccion = await _context.Direcciones.FromSqlRaw(sqlQueryDireccion).FirstOrDefaultAsync();

                var sqlQueryImagenes = $"SELECT * FROM imagenes WHERE id_alojamiento = {alojamiento.Id}";
                var imagenes = await _context.Imagenes.FromSqlRaw(sqlQueryImagenes).ToListAsync();

                var alojamientoCompleto = new AlojamientoCompleto
                {
                    Id = alojamiento.Id,
                    Nombre = alojamiento.Nombre,
                    Categoria = alojamiento.Categoria,
                    Telefono = alojamiento.Telefono,
                    Email = alojamiento.Email,
                    IdDireccion = direccion.Id,
                    Pais = direccion.Pais,
                    Calle = direccion.Calle,
                    Numero = direccion.Numero ?? 0, // Si es null, será 0 por defecto
                    CodigoPostal = direccion.CodigoPostal,
                    Provincia = direccion.Provincia,
                    Localidad = direccion.Localidad,
                    Imagenes = imagenes
                };

                alojamientosCompletos.Add(alojamientoCompleto);
            }

            return alojamientosCompletos;
        }

        // GET: api/Alojamientoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlojamientoCompleto>> GetAlojamiento(int id)
        {
            if (_context.Alojamientos == null)
            {
                return NotFound();
            }
            var alojamiento = await _context.Alojamientos.FindAsync(id);

            if (alojamiento == null)
            {
                return NotFound();
            }

            // obtenemos la direccion
            var sqlQueryDireccion = $"SELECT * FROM direcciones WHERE id = {alojamiento.IdDireccion}";
            var direccion = await _context.Direcciones.FromSqlRaw(sqlQueryDireccion).FirstOrDefaultAsync();

            // obtenemos las imagenes de la actividad
            var sqlQueryImagenes = $"SELECT * FROM imagenes WHERE id_alojamiento = {id}";
            var imagenes = await _context.Imagenes.FromSqlRaw(sqlQueryImagenes).ToListAsync();

            var alojamientoCompleto = new AlojamientoCompleto
            {
                Id = alojamiento.Id,
                Nombre = alojamiento.Nombre,
                Categoria = alojamiento.Categoria,
                Telefono = alojamiento.Telefono,
                Email = alojamiento.Email,
                IdDireccion = direccion.Id,
                Pais = direccion.Pais,
                Calle = direccion.Calle,
                Numero = direccion.Numero ?? 0, //si es null sera 0 default
                CodigoPostal = direccion.CodigoPostal,
                Provincia = direccion.Provincia,
                Localidad = direccion.Localidad,
                Imagenes = imagenes
            };

            return alojamientoCompleto;
        }

        // PUT: api/Alojamientoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlojamiento(int id, AlojamientoCompleto alojamientoCompleto)
        {
            if (id != alojamientoCompleto.Id)
            {
                throw new Exception($"{id} no es igual a {alojamientoCompleto.Id}");
            }

            // obtenemos la direccion del objeto completo
            Direccion direccion = new Direccion
            {
                Id = alojamientoCompleto.IdDireccion,
                Pais = alojamientoCompleto.Pais,
                Calle = alojamientoCompleto.Calle,
                Numero = alojamientoCompleto.Numero,
                CodigoPostal = alojamientoCompleto.CodigoPostal,
                Provincia = alojamientoCompleto.Provincia,
                Localidad = alojamientoCompleto.Localidad,
            };

            _context.Entry(direccion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // actualizar imagenes

            var sqlQueryImagenes = $"SELECT * FROM imagenes WHERE id_alojamiento = {id}";
            var imagenes = await _context.Imagenes.FromSqlRaw(sqlQueryImagenes).ToListAsync();

            foreach (var imagenExistente in alojamientoCompleto.Imagenes.ToList())
            {
                if (!alojamientoCompleto.Imagenes.Any(i => i.Id == imagenExistente.Id)) // si las imagenes de la db no existen en la lista de imagenes de la actividad a actualizar, significa que el usuario las ha borrado, y por eso las borramos
                {
                    _context.Imagenes.Remove(imagenExistente);
                }
            }

            foreach (var imagenNueva in alojamientoCompleto.Imagenes)
            {
                if (imagenNueva.Id == 0) // si el id es 0, significa que aun no está añadida a la db, y por eso es 0 de default
                {
                    _context.Imagenes.Add(imagenNueva);
                }
            }

            await _context.SaveChangesAsync();

            Alojamiento alojamiento = new Alojamiento
            {
                Id = alojamientoCompleto.Id,
                Nombre = alojamientoCompleto.Nombre,
                Categoria = alojamientoCompleto.Categoria,
                Telefono = alojamientoCompleto.Telefono,
                Email = alojamientoCompleto.Email,
                IdDireccion = alojamientoCompleto.IdDireccion
            };

            _context.Entry(alojamiento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlojamientoExists(id))
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

        // POST: api/Alojamientoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AlojamientoCrear>> PostAlojamiento(AlojamientoCrear alojamientoObjeto)
        {
          if (_context.Direcciones == null)
          {
              return Problem("Entity set 'DatabaseContext.Alojamientos'  is null.");
          }

          Direccion direccion = new Direccion
          {
              Pais = alojamientoObjeto.Pais,
              Calle = alojamientoObjeto.Calle,
              Numero = alojamientoObjeto.Numero,
              CodigoPostal = alojamientoObjeto.CodigoPostal,
              Provincia = alojamientoObjeto.Provincia,
              Localidad = alojamientoObjeto.Localidad,
           };

            _context.Direcciones.Add(direccion);
            //hago efectivo el Add en la base de datos para poder accedder al id
            await _context.SaveChangesAsync();

            int idDireccion = await _context.Direcciones
                .Where(d => d.Pais == alojamientoObjeto.Pais && d.Provincia == alojamientoObjeto.Provincia && d.Localidad == alojamientoObjeto.Localidad && d.CodigoPostal == alojamientoObjeto.CodigoPostal && d.Calle == alojamientoObjeto.Calle && d.Numero == alojamientoObjeto.Numero)
                .Select(d => d.Id)
                .FirstOrDefaultAsync();

            if (_context.Alojamientos == null)
            {
                return Problem("Entity set 'DatabaseContext.Direcciones'  is null.");
            }

            Alojamiento alojamiento = new Alojamiento
            {
                Nombre = alojamientoObjeto.Nombre,
                Categoria = alojamientoObjeto.Categoria,
                Telefono = alojamientoObjeto.Telefono,
                Email = alojamientoObjeto.Email,
                IdDireccion = idDireccion,

            };

            _context.Alojamientos.Add(alojamiento);
            await _context.SaveChangesAsync();

            //obtengo el id de la actividad que acabo de añadir para crear todas las imagenes (idActividad es un atributo -FK- necesario para crear la imagenes de actividades)
            int IdAlojamiento = await _context.Alojamientos
                .Where(a => a.Nombre == alojamientoObjeto.Nombre && a.IdDireccion == idDireccion && a.Telefono == alojamientoObjeto.Telefono)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            if (_context.Imagenes == null)
            {
                return Problem("Entity set 'DatabaseContext.Imagenes'  is null.");
            }
            //añado todas las imagenes de la actividad
            foreach (string url in alojamientoObjeto.Imagenes)
            {
                Imagen imagen = new Imagen
                {
                    Url = url,
                    IdAlojamiento = IdAlojamiento
                };
                _context.Imagenes.Add(imagen);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlojamiento", new { id = alojamiento.Id }, alojamiento);
        }

        // DELETE: api/Alojamientoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlojamiento(int id)
        {
            if (_context.Alojamientos == null)
            {
                return NotFound();
            }
            var alojamiento = await _context.Alojamientos.FindAsync(id);
            if (alojamiento == null)
            {
                return NotFound();
            }

            _context.Alojamientos.Remove(alojamiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlojamientoExists(int id)
        {
            return (_context.Alojamientos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
