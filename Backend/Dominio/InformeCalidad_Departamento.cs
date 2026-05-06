using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionCalidad.Backend.Dominio
{
    public class InformeCalidad_Departamento
    {
        [Key]
        public int id { get; set; }
        public int id_Departamento { get; set; }
        public int id_InformeCalidad { get; set; }
        public string CodigoAtencion { get; set; }
        public string Estado { get; set; } = "Activo";

        [ForeignKey("id_Departamento")]
        [JsonIgnore]
        public Departamento Departamento { get; set; }

        [ForeignKey("id_InformeCalidad")]
        [JsonIgnore]
        public InformeCalidad InformeCalidad { get; set; }
    }
}
