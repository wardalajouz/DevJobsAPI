using System.ComponentModel.DataAnnotations.Schema;

namespace DevJobsAPI.Models
{
    [Table("JobApplications")]
    public class JobApplication
    {
        public int Id { get; set; }
        public DateTime AppliedDate { get; set; } = DateTime.Now;

        // The User who applied
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // The Job they applied for
        public int JobPostingId { get; set; }

        public JobPosting JobPosting { get; set; }


        // For the Job form
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CVUrl { get; set; } // We store the link to the file
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }
}