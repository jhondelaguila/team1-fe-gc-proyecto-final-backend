using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class OfertasImagenes
{
    public int Id { get; set; }

    public int? IdImagen { get; set; }

    public int? IdOferta { get; set; }

    //public virtual Imagen? IdImagenNavigation { get; set; }

    //public virtual Oferta? IdOfertaNavigation { get; set; }
}
