using System.ComponentModel.DataAnnotations;

namespace GestionCalidad.Backend.Dominio
{
    public class Departamento
    {
        [Key]
        public int id_Departamento { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
