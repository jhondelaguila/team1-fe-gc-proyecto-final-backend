namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class ActividadCrear
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Pais { get; set; }
        public string Calle { get; set; }
        public int Numero { get; set; }
        public string CodigoPostal { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public List<string> Imagenes { get; set; }
    }
}
