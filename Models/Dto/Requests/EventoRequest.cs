using System.ComponentModel.DataAnnotations;

namespace Teatro.Models.Dto.Requests
{
    public class EventoRequest
    {
        [Required(ErrorMessage = "Titolo obbligatorio")]
        [StringLength(50)]
        public string Titolo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data obbligatoria")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Luogo obbligatorio")]
        [StringLength(50)]
        public string Luogo { get; set; } = string.Empty;

        [Required(ErrorMessage = "ArtistaId obbligatorio")]
        public Guid ArtistaId { get; set; }
    }
}