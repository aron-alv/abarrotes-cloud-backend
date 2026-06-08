namespace AbarrotesCloud.Api.DTOs
{
    public class ProductoCreateDTO
    {

        public Guid TiendaId { get; set; }
        public Guid CategoriaId { get; set; }
        public string? CodigoBarras { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public string TipoUnidad { get; set; } = null!;
    }
}
