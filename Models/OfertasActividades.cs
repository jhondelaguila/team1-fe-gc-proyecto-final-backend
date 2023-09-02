using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class OfertasActividades
{
    public int Id { get; set; }

    public int? IdOferta { get; set; }

    public int? IdActividad { get; set; }

//    public virtual Actividad? IdActividadNavigation { get; set; }

//    public virtual Oferta? IdOfertaNavigation { get; set; }
}
