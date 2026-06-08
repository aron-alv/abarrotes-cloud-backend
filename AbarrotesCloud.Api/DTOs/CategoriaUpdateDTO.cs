using System.Text.Json.Serialization;

namespace AbarrotesCloud.Api.DTOs
{
    public class CategoriaUpdateDTO
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
    }
}
