using DevJobsAPI.Models;

public interface IApplicationRepository
{
    Task<JobApplication> CreateAsync(JobApplication application);
    Task<bool> HasUserAppliedAsync(string userId, int jobId);
}