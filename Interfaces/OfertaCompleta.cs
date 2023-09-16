using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class OfertaCompleta
    {
        public Oferta Oferta { get; set; }
        public AlojamientoCompleto AlojamientoCompleto { get; set; }
        public List<ActividadCompleta>? ActividadesCompletas { get; set;}
        public List<Imagen>? Imagenes { get; set; }
    }
}
