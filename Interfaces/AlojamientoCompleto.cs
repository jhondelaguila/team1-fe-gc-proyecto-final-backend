using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class AlojamientoCompleto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public short Categoria { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public Direccion Direccion { get; set; }
        public List<Imagen> Imagenes { get; set; }
        public List<Servicio> Servicios { get; set; }
    }
}
