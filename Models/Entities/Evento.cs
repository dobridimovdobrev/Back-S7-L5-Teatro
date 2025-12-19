using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teatro.Models.Entities
{
    public class Evento
    {

        [Key]
        public Guid EventoId { get; set; }

        [Required]
        [StringLength(50)]
        public string Titolo { get; set; } = string.Empty;

        [Required]
        public DateTime Data { get; set; }

        [Required]
        [StringLength(50)]
        public string Luogo { get; set; } = string.Empty;

        [Required]
        public Guid ArtistaId { get; set; }

        [Required]
        [ForeignKey(nameof(ArtistaId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Artista? Artista { get; set; }

        public ICollection<Biglietto>? Biglietti { get; set; }
    }
}
