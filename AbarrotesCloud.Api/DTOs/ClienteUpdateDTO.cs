using System.Text.Json.Serialization;

namespace AbarrotesCloud.Api.DTOs
{
    public class ClienteUpdateDTO
    {


        [JsonPropertyName("nombreCompleto")]
        public string NombreCompleto { get; set; }
        [JsonPropertyName("telefono")]
        public string Telefono { get; set; }
        [JsonPropertyName("limiteCredito")]
        public decimal LimiteCredito { get; set; }
    }
}
