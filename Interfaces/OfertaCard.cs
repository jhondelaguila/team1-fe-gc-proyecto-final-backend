using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class OfertaCard
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public int Precio { get; set; }
        public int MaxPersonas { get; set; }
        public string? Descripcion { get; set; }
        public DateOnly FechaFin { get; set; }
        public int ofertasDisponibles { get; set; } 
        public string FotoPortada { get; set; }
    }
}

