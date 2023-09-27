namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class OfertaCrearAlojamientoAgenda
    {
        public string Titulo { get; set; }
        public int PrecioDia { get; set; }
        public int MaxPersonas { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public short OfertasDisponibles { get; set; }
        public string Descripcion { get; set; }
        public List<string> UrlFotos { get; set; }
        public int AlojamientoId { get; set; }
        public List<ActividadCrear> Actividades { get; set; }
    }
}
