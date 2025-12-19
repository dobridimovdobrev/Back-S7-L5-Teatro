using System.ComponentModel.DataAnnotations;

namespace Teatro.Models.Entity
{
    public class Artista
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Genere { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Biografia { get; set; } = string.Empty;

        public ICollection<Evento>? Eventi { get; set; }
    }
}
