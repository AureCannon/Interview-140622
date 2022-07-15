using Ct.Interview.Web.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ct.Interview.Web.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<AsxListedCompany> AsxListedCompanies { get; set; }
    }
}
