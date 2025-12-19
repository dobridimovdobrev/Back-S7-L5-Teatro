namespace Teatro.Models.Dto.Responses
{
    public class ArtistaResponse
    {
        public Guid ArtistaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Genere { get; set; } = string.Empty;
        public string Biografia { get; set; } = string.Empty;
    }
}