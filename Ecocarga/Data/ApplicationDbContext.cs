namespace Ecocarga.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Ecocarga.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TermsAndConditions> TermsAndConditions { get; set; }
        public DbSet<Bateria> Baterias { get; set; }

        public DbSet<UserAction> UserActions { get; set; }

    }
}
