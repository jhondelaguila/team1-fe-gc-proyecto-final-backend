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

        //GET: api/Alojamientoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlojamientoCard>>> GetAlojamientos()
        {
            if (_context.Alojamientos == null)
            {
                return NotFound();
            }

            var alojamientosCard = await _context.Alojamientos
                .Select(a => new AlojamientoCard
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Categoria = a.Categoria,
                    Pais = _context.Direcciones.Where(d => d.Id == a.IdDireccion).Select(d => d.Pais).FirstOrDefault(),
                    Localidad = _context.Direcciones.Where(d => d.Id == a.IdDireccion).Select(d => d.Localidad).FirstOrDefault(),
                    FotoPortada = _context.Imagenes.Where(i => i.IdAlojamiento == a.Id).Select(i => i.Url).FirstOrDefault()
                })
                .ToListAsync();

            return alojamientosCard;
        }

        // GET api/Alojamientoes/{id}
        [HttpGet("{id}/completo")]
        public async Task<ActionResult<AlojamientoCompleto>> GetAlojamientoCompleto(int id)
        {
            try
            {
                if (_context.Servicios == null)
                {
                    return NotFound();
                }

                var alojamientoCompleto = await _context.Alojamientos
                .Where(a => a.Id == id)
                .Select(a => new AlojamientoCompleto
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Categoria = a.Categoria,
                    Telefono = a.Telefono,
                    Email = a.Email,
                    Direccion = _context.Direcciones.FirstOrDefault(d => d.Id == a.IdDireccion),
                    Imagenes = _context.Imagenes
                        .Where(i => i.IdAlojamiento == a.Id)
                        .ToList(),
                    Servicios = _context.ServiciosAlojamientos
                        .Where(sa => sa.IdAlojamiento == a.Id)
                        .Select(sa => new Servicio
                        {
                            Id = sa.IdServicio,
                            Nombre = _context.Servicios.FirstOrDefault(s => s.Id == sa.IdServicio).Nombre
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

                if (alojamientoCompleto == null)
                {
                    return NotFound();
                }

                return Ok(alojamientoCompleto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        // GET: api/Alojamientoes/filtros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlojamientoFiltros>> GetAlojamiento(int id)
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

            var alojamientoCompleto = new AlojamientoFiltros
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

        // GET: api/Alojamientoes/agenda/{nombre}
        [HttpGet("agenda/{nombre}")]
        public async Task<ActionResult<AlojamientoAgenda>> GetAlojamiento(string nombre)
        {
            try
            {
                var alojamientosAgenda = await _context.Alojamientos
                    .Join(_context.Direcciones,
                        alojamiento => alojamiento.IdDireccion,
                        direccion => direccion.Id,
                        (alojamiento, direccion) => new AlojamientoAgenda
                        {
                            Id = alojamiento.Id,
                            Nombre = alojamiento.Nombre,
                            Email = alojamiento.Email,
                            Telefono = alojamiento.Telefono,
                            Direccion = new Direccion
                            {
                                Id = direccion.Id,
                                Pais = direccion.Pais,
                                Provincia = direccion.Provincia,
                                Localidad = direccion.Localidad,
                                CodigoPostal = direccion.CodigoPostal,
                                Calle = direccion.Calle,
                                Numero = direccion.Numero
                            }
                        })
                    .Where(a => a.Nombre.Contains(nombre) || a.Direccion.Pais.Contains(nombre) || a.Direccion.Localidad.Contains(nombre))
                    .ToListAsync();

                return Ok(alojamientosAgenda);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/Alojamientoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlojamiento(int id, AlojamientoFiltros alojamientoCompleto)
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
                return Problem("Entity set 'DatabaseContext.Alojamientos'  is null.");
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

            //  añadimos los servicios del hotel

            if (_context.ServiciosAlojamientos == null)
            {
                return Problem("Entity set 'DatabaseContext.Imagenes'  is null.");
            }

            foreach (int idServicio in alojamientoObjeto.Servicios)
            {
                ServiciosAlojamientos serviciosAlojamientos = new ServiciosAlojamientos
                {
                    IdServicio = idServicio,
                    IdAlojamiento = IdAlojamiento
                };
                _context.ServiciosAlojamientos.Add(serviciosAlojamientos);
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
