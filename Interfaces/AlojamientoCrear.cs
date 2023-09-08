namespace team1_fe_gc_proyecto_final_backend.Interfaces
{
    public class AlojamientoCrear
    {
        public string Nombre { get; set; }
        public short Categoria { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Pais { get; set; }
        public string Calle { get; set; }
        public int Numero { get; set; }
        public string CodigoPostal { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public List<string> Imagenes { get; set; }
    }
}
