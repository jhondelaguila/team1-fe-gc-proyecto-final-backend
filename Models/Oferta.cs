using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Oferta
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public int Precio { get; set; }

    public int MaxPersonas { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public short OfertasDisponibles { get; set; }

    public string? Descripcion { get; set; }

    public int? IdActividad { get; set; }

    public int IdAlojamiento { get; set; }

    public virtual Actividad? IdActividadNavigation { get; set; }

    public virtual Alojamiento IdAlojamientoNavigation { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<Alojamiento> IdAlojamientos { get; set; } = new List<Alojamiento>();

    public virtual ICollection<Imagen> IdImagens { get; set; } = new List<Imagen>();

    public virtual ICollection<Usuario> IdUsuarios { get; set; } = new List<Usuario>();
}
