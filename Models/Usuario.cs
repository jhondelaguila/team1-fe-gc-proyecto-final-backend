using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Nombre { get; set; } = null!;

    public string? Apellidos { get; set; }

    public string? Telefono { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string Pass { get; set; } = null!;

    public int? IdDireccion { get; set; }

    public int Puntos { get; set; }

    public int Experiencia { get; set; }

    public int? IdNivel { get; set; }

    public bool Admin { get; set; }

    //public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    //public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    //public virtual Nivel? IdNivelNavigation { get; set; }

    //public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
