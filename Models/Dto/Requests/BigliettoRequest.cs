using System.ComponentModel.DataAnnotations;

namespace Teatro.Models.Dto.Requests
{
    public class BigliettoRequest
    {
        [Required(ErrorMessage = "EventoId obbligatorio")]
        public Guid EventoId { get; set; }

        [Range(1, 10, ErrorMessage = "Quantità deve essere tra 1 e 10")]
        public int Quantita { get; set; } = 1;
    }
}