using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Nivel
{
    public string Nombre { get; set; } = null!;

    public string? Ventajas { get; set; }

    public int ExperienciaMinima { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
