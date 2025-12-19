using System.ComponentModel.DataAnnotations;

namespace Teatro.Models.Dto
{
    public class LoginRequest
    {

        [Required(ErrorMessage = "Email obbligatoria")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password obbligatoria")]
        public string Password { get; set; } = string.Empty;
    }
}
