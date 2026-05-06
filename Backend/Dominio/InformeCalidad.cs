using System.ComponentModel.DataAnnotations;

namespace GestionCalidad.Backend.Dominio
{
    public class InformeCalidad
    {
        [Key]
        public int Id_InformeCalidad { get; set; }
        public int Calificacion { get; set; }
        public string Descripcion { get; set; }
        public DateOnly Fecha { get; set; }
        public string Codigo { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}