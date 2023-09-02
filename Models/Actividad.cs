using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Actividad
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int IdDireccion { get; set; }

    //public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    //public virtual ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();

    //public virtual ICollection<OfertasActividades> OfertasActividades { get; set; } = new List<OfertasActividades>();
}
