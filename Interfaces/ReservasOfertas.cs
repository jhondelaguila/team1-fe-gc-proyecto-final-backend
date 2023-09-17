using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class ReservasOfertas
    {
        public int IdResrva { get; set; }

        public string TituloOferta { get; set; }
        public DateOnly FechaIni { get; set; }
        public DateOnly FechaFinal { get; set; }
        public Direccion Direccion { get; set; }
        public string Estado { get; set; }
        public int PrecioOferta { get; set; }
        public string ImagenOferta { get; set; }
    }
}