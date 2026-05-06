using System.Text.Json.Serialization;

namespace GestionCalidad.Backend.DTO.Paciente
{
    public class PacienteDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("apellido_p")]
        public string ApellidoP { get; set; }

        [JsonPropertyName("apellido_m")]
        public string ApellidoM { get; set; }
    }
}
