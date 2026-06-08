namespace AbarrotesCloud.Api.DTOs
{
    public class ClienteCreateDTO
    {
        public Guid TiendaId { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public decimal LimiteCredito { get; set; }
        public decimal SaldoActual { get; set; }


    }
}
