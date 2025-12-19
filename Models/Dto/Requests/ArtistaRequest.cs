using System.ComponentModel.DataAnnotations;

namespace Teatro.Models.Dto.Requests
{
    public class ArtistaRequest
    {
        [Required(ErrorMessage = "Nome obbligatorio")]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Genere obbligatorio")]
        [StringLength(50)]
        public string Genere { get; set; } = string.Empty;

        [Required(ErrorMessage = "Biografia obbligatoria")]
        [StringLength(1000)]
        public string Biografia { get; set; } = string.Empty;
    }
}