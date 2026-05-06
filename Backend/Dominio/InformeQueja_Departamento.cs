using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionCalidad.Backend.Dominio
{
    public class InformeQueja_Departamento
    {
        [Key]
        public int id { get; set; }
        public int id_InformeQueja { get; set; }
        public int id_Departamento { get; set; }
        public string CodigoPaciente { get; set; }
        public string Estado { get; set; } = "Activo";

        [ForeignKey("id_Departamento")]
        [JsonIgnore]
        public Departamento Departamento { get; set; }

        [ForeignKey("id_InformeQueja")]
        [JsonIgnore]
        public InformeQueja InformeQueja { get; set; }
    }
}
