namespace GestionCalidad.Backend.DTO.InformeCalidad
{
    public class InformeCalidadDTO
    {
        public int Calificacion { get; set; }
        public string Descripcion { get; set; }
        public DateOnly Fecha { get; set; }
        public string Codigo { get; set; }
    }
}
