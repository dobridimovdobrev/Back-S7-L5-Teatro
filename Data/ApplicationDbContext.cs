using Teatro.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Teatro.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Artista> Artisti { get; set; }
        public DbSet<Biglietto> Biglietti { get; set; }
        public DbSet<Evento> Eventi { get; set; }
    }
}
