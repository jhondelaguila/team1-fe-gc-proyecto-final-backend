using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class ActividadCard
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Pais { get; set; }
        public string Localidad { get; set; }
        public string FotoPortada { get; set; }
    }
}
