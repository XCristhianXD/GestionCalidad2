using System.ComponentModel.DataAnnotations;

namespace GestionCalidad.Backend.Dominio
{
    public class InformeQueja
    {
        [Key]
        public int id_InformeQueja { get; set; }
        public string Descripcion { get; set; }
        public DateOnly Fecha { get; set; }
        public string Codigo { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
