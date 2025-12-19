using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teatro.Models.Entities;

namespace Teatro.Models.Entities
{
    public class Biglietto
    {
        [Key]
        public Guid BigliettoId { get; set; }

        [Required]
        public DateTime DataAcquisto { get; set; }

        [Required]
        public Guid EventoId { get; set; }

        [Required]
        [ForeignKey(nameof(EventoId))]

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Evento? Evento { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public ApplicationUser? User { get; set; }
    }
}
