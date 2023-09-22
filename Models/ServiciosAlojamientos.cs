using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class ServiciosAlojamientos
{
    public int Id { get; set; }

    public int IdServicio { get; set; }

    public int IdAlojamiento { get; set; }

    //public virtual Alojamiento? IdAlojamientoNavigation { get; set; }

    //public virtual Servicio? IdServicioNavigation { get; set; }
}
