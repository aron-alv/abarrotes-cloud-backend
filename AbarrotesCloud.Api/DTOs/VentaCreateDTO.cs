namespace AbarrotesCloud.Api.DTOs
{
    public class VentaCreateDTO
    {
        public Guid TiendaId { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid? ClienteCreditoId { get; set; }

        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = null!;

        public List<DetalleVentaCreateDTO> Detalles { get; set; } = new List<DetalleVentaCreateDTO>();

    }
    public class DetalleVentaCreateDTO
    {
        public Guid VentaId { get; set; }
        public Guid ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
