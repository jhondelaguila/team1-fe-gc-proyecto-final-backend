using System;
using System.Collections.Generic;

namespace team1_fe_gc_proyecto_final_backend.Models;

public partial class Direccion
{
    public int Id { get; set; }

    public string Pais { get; set; } = null!;

    public string Provincia { get; set; } = null!;

    public string Localidad { get; set; } = null!;

    public string CodigoPostal { get; set; } = null!;

    public string Calle { get; set; } = null!;

    public int? Numero { get; set; }


    //    public virtual ICollection<Actividad> Actividades { get; set; } = new List<Actividad>();

    //    public virtual ICollection<Alojamiento> Alojamientos { get; set; } = new List<Alojamiento>();

    //    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    
    public Direccion() { }

    public Direccion(
        string pais,
        string provincia,
        string localidad,
        string codigoPostal,
        string calle,
        int? numero)
    {
        Pais = pais;
        Provincia = provincia;
        Localidad = localidad;
        CodigoPostal = codigoPostal;
        Calle = calle;
        Numero = numero;
    }
}
