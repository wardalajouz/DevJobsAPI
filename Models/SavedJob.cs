namespace DevJobsAPI.Models
{
    public class SavedJob
    {
        public int JobId { get; set; }

        public string AppUserId { get; set; }

        // navigation properties , We add the actual AppUser and JobPosting objects so Entity Framework can "hop" between tables easily.
        public AppUser AppUser { get; set; }

        public JobPosting JobPosting { get; set; }
    }
}
