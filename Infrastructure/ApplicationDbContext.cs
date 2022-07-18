using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts)
            : base(opts)
        {
        }

        public DbSet<AsxListedCompany> Companies { get; set; }
    }
}
