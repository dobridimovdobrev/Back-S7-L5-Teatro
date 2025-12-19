namespace Teatro.Models.Dto.Responses
{
    public class BigliettoResponse
    {
        public Guid BigliettoId { get; set; }
        public DateTime DataAcquisto { get; set; }
        public Guid EventoId { get; set; }
        public string TitoloEvento { get; set; } = string.Empty;
        public DateTime DataEvento { get; set; }
        public string LuogoEvento { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string NomeUtente { get; set; } = string.Empty;
    }
}