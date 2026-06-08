namespace AbarrotesCloud.Api.DTOs
{
    public class CategoriaCreateDTO
    {

        public Guid TiendaId { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
