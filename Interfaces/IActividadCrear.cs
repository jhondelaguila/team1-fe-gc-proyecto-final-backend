namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public interface IActividadCrear
    {
        string Titulo { get; set; }
        string Descripcion { get; set; }
        string Pais { get; set; }
        string Calle { get; set; }
        int Numero { get; set; }
        string CodigoPostal { get; set; }
        string Provincia { get; set; }
        string Localidad { get; set; }
        List<string> Imagenes { get; set; }
    }

    public class ActividadCrear : IActividadCrear
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
