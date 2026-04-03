using DevJobsAPI.Models;

namespace DevJobsAPI.Interfaces
{
    public interface ISavedJobRepository
    {

        Task<List<JobPosting>> GetUserSavedJobs(AppUser user);
        Task <SavedJob> CreateAsync(SavedJob savedJob);

        Task<SavedJob> DeleteAsync(AppUser user, int jobId );
    }
}
