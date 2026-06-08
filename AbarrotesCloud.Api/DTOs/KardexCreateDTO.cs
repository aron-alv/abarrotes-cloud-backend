namespace AbarrotesCloud.Api.DTOs
{
    public class KardexCreateDTO
    {

        public Guid TiendaId { get; set; }
        public Guid ProductoId { get; set; }
        public Guid UsuarioId { get; set; }
        public string TipoMovimiento { get; set; } = null!;
        public string Motivo { get; set; }
        public decimal Cantidad { get; set; }
    }
}
