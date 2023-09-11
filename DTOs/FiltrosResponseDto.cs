using System;
using team1_fe_gc_proyecto_final_backend.Models;
namespace team1_fe_gc_proyecto_final_backend.DTOs
{
	public class FiltrosResponseDto
	{
		public List<Oferta> Ofertas { get; set; }
		public List<Alojamiento> Alojamientos { get; set; }
		public List<ServiciosAlojamientos> S_A { get; set; }
	}
}

