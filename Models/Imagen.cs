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

    //public virtual Actividad? IdActividadNavigation { get; set; }

    //public virtual Alojamiento? IdAlojamientoNavigation { get; set; }

    //public virtual ICollection<OfertasImagenes> OfertasImagenes { get; set; } = new List<OfertasImagenes>();

    public Imagen() { }

    public Imagen(string Url, int IdActividad) 
    { 
        this.Url = Url;
        this.IdActividad = IdActividad;
    }
}
