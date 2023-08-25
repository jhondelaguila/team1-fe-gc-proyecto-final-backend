using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Servicio
{
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Alojamiento> IdAlojamientos { get; set; } = new List<Alojamiento>();
}
