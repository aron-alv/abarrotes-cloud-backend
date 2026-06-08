namespace AbarrotesCloud.Api.DTOs
{
    public class AbonoCreditoCreateDTO
    {
        public Guid ClienteId { get; set; }
        public Guid TiendaId { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal MontoAbonado { get; set; }

        public DateTime? Fechaabono { get; set; }

    }
}
