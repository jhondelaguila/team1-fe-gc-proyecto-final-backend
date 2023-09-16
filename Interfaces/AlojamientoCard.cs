namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class AlojamientoCard
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int Categoria { get; set; }
        public string Pais { get; set; }
        public string Localidad { get; set; }
        public string FotoPortada { get; set; }
    }
}
