using DevJobsAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace DevJobsAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // You don't want to manually type "Admin" and "User" into your database every time. We "Seed" them here so they are created automatically.
            base.OnModelCreating(builder);

            // Hardcoded IDs and ConcurrencyStamps to keep EF Core 9/10 happy
            var adminRoleId = "4839c916-3e3c-4161-9c60-80d859d95513";
            var userRoleId = "f549c664-8848-43e3-8f52-87009405f624";

            List<IdentityRole> roles = new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = adminRoleId,
            ConcurrencyStamp = adminRoleId, // Use the ID as the stamp
            Name = "Admin",
            NormalizedName = "ADMIN"
        },
        new IdentityRole
        {
            Id = userRoleId,
            ConcurrencyStamp = userRoleId, // Use the ID as the stamp
            Name = "User",
            NormalizedName = "USER"
        }
    };

            // here we tell the EF that the unique identifier is the combination of both ids (jopposting id and appuser id)
            builder.Entity<IdentityRole>().HasData(roles);


            builder.Entity<SavedJob>(x => x.HasKey(p => new { p.AppUserId, p.JobId }));

            builder.Entity<SavedJob>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.SavedJobs)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<SavedJob>()
                .HasOne(u => u.JobPosting)
                .WithMany(u => u.SavedJobs)
                .HasForeignKey(p => p.JobId);
        }

     
       

        public DbSet<JobPosting> JobPostings { get; set; } // this line tells the ef core to create a table for the JobPosting table based on the JobPosting class in the Models folder

        public DbSet<SavedJob> SavedJobs { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        
    }
}
