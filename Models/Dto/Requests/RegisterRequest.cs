using System.ComponentModel.DataAnnotations;

namespace Teatro.Models.Dto.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Nome obbligatorio")]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cognome obbligatorio")]
        [StringLength(50)]
        public string Cognome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email obbligatoria")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password obbligatoria")]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data di nascita obbligatoria")]
        public DateOnly DataDiNascita { get; set; }
    }
}