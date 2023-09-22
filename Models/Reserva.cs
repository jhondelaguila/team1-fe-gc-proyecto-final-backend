using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Reserva
{
    public int Id { get; set; }

    public int? IdOferta { get; set; }

    public int? IdUsuario { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public string? Estado { get; set; }

    //public virtual Oferta? IdOfertaNavigation { get; set; }

    //public virtual Usuario? IdUsuarioNavigation { get; set; }
}
