using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Alojamiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public short Categoria { get; set; }

    public string Telefono { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int IdDireccion { get; set; }

    //public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    //public virtual ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();

    //public virtual ICollection<Oferta> Oferta { get; set; } = new List<Oferta>();

    //public virtual ICollection<ServiciosAlojamientos> ServiciosAlojamientos { get; set; } = new List<ServiciosAlojamientos>();
}
