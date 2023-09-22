using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class OfertasActividades
{
    public int Id { get; set; }

    public int IdOferta { get; set; }

    public int IdActividad { get; set; }

    public OfertasActividades(int IdOferta, int IdActividad)
    {
        this.IdOferta = IdOferta;
        this.IdActividad = IdActividad;
    }
    public OfertasActividades() { }
}
