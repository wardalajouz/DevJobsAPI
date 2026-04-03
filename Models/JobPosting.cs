using System.ComponentModel.DataAnnotations.Schema;

namespace DevJobsAPI.Models
{
    public class JobPosting
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        // =string.Empty ; is usedto initialize the string properties to an empty string, so that they are not null when we create a new instance of the JobPosting class.

        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public List<SavedJob> SavedJobs { get; set; }= new List<SavedJob>(); // This tells Entity Framework: "Hey, if I have a User object, I want to be able to see a list of all the rows in the SavedJobs table that belong to them



        // the "owner" of the job ( to prevent other users to delete each other job posting ) , we only need the one who created the job to be able to delete it 
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

    }
}
