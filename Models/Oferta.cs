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

    public int IdAlojamiento { get; set; }

    //public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    //public virtual Alojamiento IdAlojamientoNavigation { get; set; } = null!;

    //public virtual ICollection<OfertasActividades> OfertasActividades { get; set; } = new List<OfertasActividades>();

    //public virtual ICollection<OfertasImagenes> OfertasImagenes { get; set; } = new List<OfertasImagenes>();

    //public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
