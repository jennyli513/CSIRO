using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSIRO.Models
{
    public class CandidateDataContext: IdentityDbContext

    {
        public DbSet<Candidate> candidate { get; set; }
        public DbSet<Course> course { get; set; }
        public DbSet<University> university { get; set; }

      

        public CandidateDataContext(DbContextOptions<CandidateDataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
