using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Usuario
{
    public string Email { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Pass { get; set; } = null!;

    public int IdDireccion { get; set; }

    public int? Puntos { get; set; }

    public int? Experiencia { get; set; }

    public string? Nivel { get; set; }

    public bool Admin { get; set; }

    //public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    //public virtual Nivel? NivelNavigation { get; set; }

    //public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    //public virtual ICollection<Oferta> IdOferta { get; set; } = new List<Oferta>();
}
