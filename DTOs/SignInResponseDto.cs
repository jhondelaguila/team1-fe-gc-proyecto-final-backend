using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.DTOs
{
    public class SignInResponseDto
    {
        public string Token { get; set; }
        public Usuario Usuario { get; set; }
    }
}
