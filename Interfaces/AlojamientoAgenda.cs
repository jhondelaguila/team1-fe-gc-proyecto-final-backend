using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class AlojamientoAgenda
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int Categoria { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public Direccion Direccion { get; set; }
        
    }
}
