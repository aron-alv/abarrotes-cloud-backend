using System.Text.Json.Serialization;

namespace AbarrotesCloud.Api.DTOs 
{
    public class ProductoUpdateDTO
    {
        [JsonPropertyName("categoriaId")]
        public Guid CategoriaId { get; set; }

        
        [JsonPropertyName("codigoBarras")]
        public string? CodigoBarras { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("precioCompra")]
        public decimal PrecioCompra { get; set; }

        [JsonPropertyName("precioVenta")]
        public decimal PrecioVenta { get; set; }

        
        [JsonPropertyName("tipoUnidad")]
        public string TipoUnidad { get; set; }

        [JsonPropertyName("stockMinimo")]
        public int StockMinimo { get; set; }
    }
}