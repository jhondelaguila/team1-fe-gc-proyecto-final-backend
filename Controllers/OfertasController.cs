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
using team1_fe_gc_proyecto_final_backend.Interfaces;
using Ubiety.Dns.Core;
//using Google.Protobuf.Collections;

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

        // GET: api/Ofertas/home
        [HttpGet("Home")]
        public async Task<IActionResult> GetOfertasCard()
        {
            try
            {

                var ofertas = await _context.Ofertas
                    .OrderByDescending(o => o.FechaFin) 
                    .Select(o => new OfertaCard
                    {
                        Id = o.Id,
                        Titulo = o.Titulo,
                        Precio = o.Precio,
                        MaxPersonas = o.MaxPersonas,
                        Descripcion = o.Descripcion,
                        FechaFin = o.FechaFin,
                        FotoPortada = _context.OfertasImagenes
                            .Where(oi => oi.IdOferta == o.Id).Join(_context.Imagenes, oi => oi.IdImagen, i => i.Id, (oi, i) => i.Url) .FirstOrDefault()
                    })
                    .ToListAsync();

                return Ok(ofertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Ofertas
        [HttpGet("Filtros")]
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

        //GET: api/Ofertas
       [HttpGet("FiltrosCard")]
        public async Task<ActionResult<OfertaFiltro>> GetOfertasFiltrosCard()
        {
            var ofertasCompletas = await _context.Ofertas
                .Select(o => new OfertaFiltro
                {
                    Oferta = new OfertaCard
                    {
                        Id = o.Id,
                        Titulo = o.Titulo,
                        Precio = o.Precio,
                        MaxPersonas = o.MaxPersonas,
                        Descripcion = o.Descripcion,
                        FechaFin = o.FechaFin,
                        FotoPortada = _context.OfertasImagenes
                            .Where(oi => oi.IdOferta == o.Id)
                            .Join(_context.Imagenes, oi => oi.IdImagen, i => i.Id, (oi, i) => i.Url)
                            .FirstOrDefault()
                    },
                    CategoriaAlojamiento = _context.Alojamientos
                        .Where(a => a.Id == o.IdAlojamiento)
                        .Select(a => a.Categoria)
                        .FirstOrDefault(),
                    DireccionAlojamiento = _context.Alojamientos
                        .Where(a => a.Id == o.IdAlojamiento)
                        .Join(
                            _context.Direcciones,
                            alojamiento => alojamiento.IdDireccion,
                            direccion => direccion.Id,
                            (alojamiento, direccion) => new Direccion
                            {
                                Id = direccion.Id,
                                Pais = direccion.Pais,
                                Provincia = direccion.Provincia,
                                Localidad = direccion.Localidad,
                                CodigoPostal = direccion.CodigoPostal,
                                Calle = direccion.Calle,
                                Numero = direccion.Numero
                            }
                        )
                        .FirstOrDefault(),
                    ServiciosAlojamiento = _context.ServiciosAlojamientos
                        .Where(sa => sa.IdAlojamiento == o.Id)
                        .Join(_context.Servicios, sa => sa.IdServicio, s => s.Id, (sa, s) => new Servicio
                        {
                            Id = s.Id,
                            Nombre = s.Nombre
                        })
                        .ToList()
                })
                .ToListAsync();


            return Ok(ofertasCompletas);
        }

        // GET: api/Ofertas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfertaCompleta>> GetOferta(int id)
        {
            if (_context.Ofertas == null)
            {
                return NotFound();
            }

            var ofertaCompleta = await _context.Ofertas
                .Where(o => o.Id == id)
                .Select(o => new OfertaCompleta
                {
                    Oferta = new Oferta
                    {
                        Id = o.Id,
                        Titulo = o.Titulo,
                        Precio = o.Precio,
                        MaxPersonas = o.MaxPersonas,
                        FechaInicio = o.FechaInicio,
                        FechaFin = o.FechaFin,
                        OfertasDisponibles = o.OfertasDisponibles,
                        Descripcion = o.Descripcion,
                        IdAlojamiento = o.IdAlojamiento,
                    },

                    Alojamiento = _context.Alojamientos
                        .Where(a => a.Id == o.IdAlojamiento)
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
                        .FirstOrDefault(),
                    Actividades = _context.OfertasActividades
                        .Where(oa => oa.IdOferta == o.Id)
                        .Join(_context.Actividades, oa => oa.IdActividad, a => a.Id, (oi, a) => new ActividadCompleta
                        {
                            Id = a.Id,
                            Titulo = a.Titulo,
                            Descripcion = a.Descripcion,
                            Direccion = _context.Direcciones.FirstOrDefault(d => d.Id == a.IdDireccion),
                            Imagenes = _context.Imagenes
                                .Where(i => i.IdActividad == a.Id)
                                .ToList()
                        })
                        .ToList(),
                    Imagenes = _context.OfertasImagenes
                        .Where(oi => oi.IdOferta == o.Id)
                        .Join(_context.Imagenes, oi => oi.IdImagen, i => i.Id, (oi, i) => new Imagen
                        {
                            Id = i.Id,
                            Url = i.Url,
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();


            if (ofertaCompleta == null)
            {
                return NotFound();
            }

            return ofertaCompleta;
        }


        [HttpGet("Buscar")]
        public async Task<ActionResult<IEnumerable<OfertaFiltro>>> GetBuscarOfertas(
            [FromQuery(Name = "ubicacion")] string? ubicacion,
            [FromQuery(Name = "fecha_inicio")] string? fecha_inicio,
            [FromQuery(Name = "fecha_fin")] string? fecha_fin,
            [FromQuery(Name = "num_personas")] int? num_personas)
        {
            var query = _context.Ofertas
                .Join(
                    _context.Alojamientos,
                    o => o.IdAlojamiento,
                    a => a.Id,
                    (o, a) => new { Oferta = o, Alojamiento = a }
                )
                .Join(
                    _context.Direcciones,
                    oa => oa.Alojamiento.IdDireccion,
                    d => d.Id,
                    (oa, d) => new { OfertaAlojamiento = oa, Direccion = d }
                )
                .Where(oa =>
                    ((string.IsNullOrEmpty(ubicacion) || oa.Direccion.Localidad.ToLower().Contains(ubicacion.ToLower())) || oa.Direccion.Pais.ToLower().Contains(ubicacion.ToLower()) || oa.Direccion.Provincia.ToLower().Contains(ubicacion.ToLower())) &&
                    (string.IsNullOrEmpty(fecha_inicio) || DateOnly.Parse(fecha_inicio) >= oa.OfertaAlojamiento.Oferta.FechaInicio) &&
                    (string.IsNullOrEmpty(fecha_fin) || DateOnly.Parse(fecha_fin) <= oa.OfertaAlojamiento.Oferta.FechaFin) &&
                    (!num_personas.HasValue || num_personas <= oa.OfertaAlojamiento.Oferta.MaxPersonas)
                )
                .Select(oa => new OfertaFiltro
                {
                    Oferta = new OfertaCard
                    {
                        Id = oa.OfertaAlojamiento.Oferta.Id,
                        Titulo = oa.OfertaAlojamiento.Oferta.Titulo,
                        Precio = oa.OfertaAlojamiento.Oferta.Precio,
                        MaxPersonas = oa.OfertaAlojamiento.Oferta.MaxPersonas,
                        Descripcion = oa.OfertaAlojamiento.Oferta.Descripcion,
                        FechaFin = oa.OfertaAlojamiento.Oferta.FechaFin,
                        FotoPortada = _context.OfertasImagenes
                            .Where(oi => oi.IdOferta == oa.OfertaAlojamiento.Oferta.Id)
                            .Join(_context.Imagenes, oi => oi.IdImagen, i => i.Id, (oi, i) => i.Url)
                            .FirstOrDefault()
                    },
                    CategoriaAlojamiento = oa.OfertaAlojamiento.Alojamiento.Categoria,
                    DireccionAlojamiento = new Direccion
                    {
                        Pais = oa.Direccion.Pais,
                        Provincia = oa.Direccion.Provincia,
                        Localidad = oa.Direccion.Localidad,
                        CodigoPostal = oa.Direccion.CodigoPostal,
                        Calle = oa.Direccion.Calle,
                        Numero = oa.Direccion.Numero
                    },
                    ServiciosAlojamiento = _context.ServiciosAlojamientos
                        .Where(sa => sa.IdAlojamiento == oa.OfertaAlojamiento.Alojamiento.Id)
                        .Join(_context.Servicios, sa => sa.IdServicio, s => s.Id, (sa, s) => new Servicio
                        {
                            Id = s.Id,
                            Nombre = s.Nombre
                        })
                        .ToList()
                });

            var ofertasFiltradas = await query.ToListAsync();

            return Ok(ofertasFiltradas);
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
        public async Task<ActionResult<OfertaCrear>> PostOferta(OfertaCrear ofertaCrear)
        {

            if (_context.Direcciones == null)
            {
                return Problem("Entity set 'DatabaseContext.Alojamientos'  is null.");
            }

            Direccion direccion = new Direccion
            {
                Pais = ofertaCrear.Alojamiento.Pais,
                Calle = ofertaCrear.Alojamiento.Calle,
                Numero = ofertaCrear.Alojamiento.Numero,
                CodigoPostal = ofertaCrear.Alojamiento.CodigoPostal,
                Provincia = ofertaCrear.Alojamiento.Provincia,
                Localidad = ofertaCrear.Alojamiento.Localidad,
            };

            _context.Direcciones.Add(direccion);
            //hago efectivo el Add en la base de datos para poder accedder al id
            await _context.SaveChangesAsync();

            int idDireccion = await _context.Direcciones
                .Where(d => d.Pais == ofertaCrear.Alojamiento.Pais && d.Provincia == ofertaCrear.Alojamiento.Provincia && d.Localidad == ofertaCrear.Alojamiento.Localidad && d.CodigoPostal == ofertaCrear.Alojamiento.CodigoPostal && d.Calle == ofertaCrear.Alojamiento.Calle && d.Numero == ofertaCrear.Alojamiento.Numero)
                .Select(d => d.Id)
                .FirstOrDefaultAsync();

            if (_context.Alojamientos == null)
            {
                return Problem("Entity set 'DatabaseContext.Direcciones'  is null.");
            }

            Alojamiento alojamiento = new Alojamiento
            {
                Nombre = ofertaCrear.Alojamiento.Nombre,
                Categoria = ofertaCrear.Alojamiento.Categoria,
                Telefono = ofertaCrear.Alojamiento.Telefono,
                Email = ofertaCrear.Alojamiento.Email,
                IdDireccion = idDireccion,
            };

            _context.Alojamientos.Add(alojamiento);
            await _context.SaveChangesAsync();

            //obtengo el id de la alojamiento que acabo de añadir para crear todas las imagenes (idActividad es un atributo -FK- necesario para crear la imagenes de alojamiento)
            int IdAlojamiento = await _context.Alojamientos
                .Where(a => a.Nombre == ofertaCrear.Alojamiento.Nombre && a.IdDireccion == idDireccion && a.Telefono == ofertaCrear.Alojamiento.Telefono)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            if (_context.Imagenes == null)
            {
                return Problem("Entity set 'DatabaseContext.Imagenes'  is null.");
            }
            //añado todas las imagenes del alojamiento
            foreach (string url in ofertaCrear.Alojamiento.Imagenes)
            {
                Imagen imagen = new Imagen
                {
                    Url = url,
                    IdAlojamiento = IdAlojamiento
                };
                _context.Imagenes.Add(imagen);
            }

            if (_context.ServiciosAlojamientos == null)
            {
                return Problem("Entity set 'DatabaseContext.Imagenes'  is null.");
            }

            foreach (int idServicio in ofertaCrear.Alojamiento.Servicios)
            {
                ServiciosAlojamientos serviciosAlojamientos = new ServiciosAlojamientos
                {
                    IdServicio = idServicio,
                    IdAlojamiento = IdAlojamiento
                };
                _context.ServiciosAlojamientos.Add(serviciosAlojamientos);
            }

            await _context.SaveChangesAsync();

            // CREAR OFERTA

            if (_context.Ofertas == null)
            {
                return Problem("Entity set 'DatabaseContext.Ofertas'  is null.");
            }

            Oferta oferta = new Oferta
            {
                Titulo = ofertaCrear.Titulo,
                Precio = ofertaCrear.PrecioDia,
                MaxPersonas = ofertaCrear.MaxPersonas,
                FechaInicio = ofertaCrear.FechaInicio,
                FechaFin = ofertaCrear.FechaFin,
                OfertasDisponibles = ofertaCrear.OfertasDisponibles,
                Descripcion = ofertaCrear.Descripcion,
                IdAlojamiento = IdAlojamiento
            };

            _context.Ofertas.Add(oferta);
            await _context.SaveChangesAsync();

            // obtenemos el id de oferta para las relaciones con fotos y actividades

            int idOferta = await _context.Ofertas
               .Where(o => o.Titulo == oferta.Titulo && o.Precio == oferta.Precio && o.MaxPersonas == oferta.MaxPersonas && o.FechaInicio == oferta.FechaInicio && o.FechaFin == oferta.FechaFin && o.IdAlojamiento == oferta.IdAlojamiento)
               .Select(o => o.Id)
               .FirstOrDefaultAsync();

            // CREAR ACTIVIDAD

            foreach (ActividadCrear actividadOferta in ofertaCrear.Actividades)
            {
                Direccion direccionActividad = new Direccion(actividadOferta.Pais, actividadOferta.Provincia, actividadOferta.Localidad, actividadOferta.CodigoPostal, actividadOferta.Calle, actividadOferta.Numero);
                _context.Direcciones.Add(direccionActividad);

                await _context.SaveChangesAsync();

                int idDireccionActividad = await _context.Direcciones
               .Where(d => d.Pais == actividadOferta.Pais && d.Provincia == actividadOferta.Provincia && d.Localidad == actividadOferta.Localidad && d.CodigoPostal == actividadOferta.CodigoPostal && d.Calle == actividadOferta.Calle && d.Numero == actividadOferta.Numero)
               .Select(d => d.Id)
               .FirstOrDefaultAsync();

                if (_context.Actividades == null)
                {
                    return Problem("Entity set 'DatabaseContext.Direcciones'  is null.");
                }

                //Obtengo la actividad del objeto que me llega del front
                Actividad actividad = new Actividad(actividadOferta.Titulo, actividadOferta.Descripcion, idDireccionActividad);
                _context.Actividades.Add(actividad);
                await _context.SaveChangesAsync();

                //obtengo el id de la actividad que acabo de añadir para crear todas las imagenes (idActividad es un atributo -FK- necesario para crear la imagenes de actividades)
                int IdActividad = await _context.Actividades
                    .Where(a => a.Titulo == actividadOferta.Titulo && a.Descripcion == actividadOferta.Descripcion && a.IdDireccion == idDireccionActividad)
                    .Select(a => a.Id)
                    .FirstOrDefaultAsync();

                if (_context.Imagenes == null)
                {
                    return Problem("Entity set 'DatabaseContext.Imagenes'  is null.");
                }
                //añado todas las imagenes de la actividad
                foreach (string url in actividadOferta.Imagenes)
                {
                    Imagen imagen = new Imagen
                    {
                        Url = url,
                        IdActividad = IdActividad
                    };
                    _context.Imagenes.Add(imagen);
                }

                // RELACION OFERTAS ACTIVIDADES

                OfertasActividades ofertaActividad = new OfertasActividades
                {
                    IdActividad = IdActividad,
                    IdOferta = idOferta
                };
                _context.OfertasActividades.Add(ofertaActividad);


                await _context.SaveChangesAsync();
            }
            //Obtengo y añado la direccion del objeto que me llega del front
            

            //hago efectivo el Add en la base de datos para poder accedder al id
            await _context.SaveChangesAsync();

            // RELACION ACTIVIDADES IMAGENES

            foreach (string url in ofertaCrear.UrlFotos)
            {
                Imagen imagen = new Imagen
                {
                    Url = url
                };

                _context.Imagenes.Add(imagen);
                await _context.SaveChangesAsync();

                // obtengo id de la imagen para la realcioon con ofertas 

                int IdImagenOferta = await _context.Imagenes
                    .Where(i => i.Url == url)
                    .Select(i => i.Id)
                    .FirstOrDefaultAsync();



                OfertasImagenes ofertasImagenes = new OfertasImagenes
                {
                    IdImagen = IdImagenOferta,
                    IdOferta = idOferta
                };

                _context.OfertasImagenes.Add(ofertasImagenes);

                await _context.SaveChangesAsync();
            }

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
