using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Teatro.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Cognome { get; set; } = string.Empty;

        public DateOnly DataDiNascita { get; set; }
        public DateTime DataCreazione { get; set; }
        public bool Active { get; set; }
        public ICollection<Biglietto>? Biglietti { get; set; }

    }
}
