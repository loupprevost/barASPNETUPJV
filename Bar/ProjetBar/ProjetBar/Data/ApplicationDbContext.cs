using Bar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjetBar.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Article>? Article { get; set; }
        public DbSet<Barman>? Barman { get; set; }
        public DbSet<CoContenu>? CoContenu { get; set; }
        public DbSet<Commande>? Commande { get; set; }
        public DbSet<LstTable>? LstTable { get; set; }
        public DbSet<Serveur>? Serveur { get; set; }
        public DbSet<Caissier>? Caissier { get; set; }
        public DbSet<Administrateur>? Administrateur { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}