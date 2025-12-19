namespace Teatro.Models.Dto.Responses
{
    public class EventoResponse
    {
        public Guid EventoId { get; set; }
        public string Titolo { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Luogo { get; set; } = string.Empty;
        public Guid ArtistaId { get; set; }
        public string NomeArtista { get; set; } = string.Empty;
    }
}