using Microsoft.EntityFrameworkCore;
using DevJobsAPI.Models;

namespace DevJobsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
       
        public DbSet<JobPosting> JobPostings { get; set; } // this line tells the ef core to create a table for the JobPosting table based on the JobPosting class in the Models folder

    }
}
