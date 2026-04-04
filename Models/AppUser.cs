using Microsoft.AspNetCore.Identity;
using DevJobsAPI.Models;

namespace DevJobsAPI.Models
{
    public class AppUser : IdentityUser
    {
        // Personal Information
        public string FirstName { get; set; } 
        public string? FullName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Bio { get; set; }

        // Professional Links
        public string? GitHubUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? PortfolioUrl { get; set; }

        // Profile Details
        public string? ProfilePictureUrl { get; set; }

        // Relationship: One user can post many jobs (if they are a recruiter)
        public List<JobPosting> JobPostings { get; set; } = new List<JobPosting>();

        public List<SavedJob> SavedJobs { get; set; }=new List<SavedJob>(); // This tells Entity Framework: "Hey, if I have a User object, I want to be able to see a list of all the rows in the SavedJobs table that belong to them
    }
}