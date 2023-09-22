using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class ActividadCompleta
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Direccion Direccion { get; set; }
        public List<Imagen> Imagenes { get; set; }
    }
}
