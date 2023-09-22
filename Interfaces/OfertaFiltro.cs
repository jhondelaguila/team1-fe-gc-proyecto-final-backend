using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class OfertaFiltro
    {
        public OfertaCard Oferta { get; set; }
        public int CategoriaAlojamiento { get; set; }
        public Direccion DireccionAlojamiento { get; set; }
        public List<Servicio> ServiciosAlojamiento { get; set; }
    }

}

