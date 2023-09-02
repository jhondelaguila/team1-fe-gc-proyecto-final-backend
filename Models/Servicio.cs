using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Servicio
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    //public virtual ICollection<ServiciosAlojamientos> ServiciosAlojamientos { get; set; } = new List<ServiciosAlojamientos>();
}
