using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Imagen
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public int? IdActividad { get; set; }

    public int? IdAlojamiento { get; set; }

    public string? Ubicacion { get; set; }

    public virtual Actividad? IdActividadNavigation { get; set; }

    public virtual Alojamiento? IdAlojamientoNavigation { get; set; }

    public virtual ICollection<Oferta> IdOferta { get; set; } = new List<Oferta>();
}
