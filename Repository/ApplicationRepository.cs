using DevJobsAPI.Data;
using DevJobsAPI.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationRepository : IApplicationRepository
{
    private readonly ApplicationDbContext _context;
    public ApplicationRepository(ApplicationDbContext context) => _context = context;

    public async Task<JobApplication> CreateAsync(JobApplication application)
    {
        await _context.JobApplications.AddAsync(application);
        await _context.SaveChangesAsync();
        return application;
    }

    public async Task<bool> HasUserAppliedAsync(string userId, int jobId)
    {
        return await _context.JobApplications.AnyAsync(a => a.AppUserId == userId && a.JobPostingId == jobId);
    }
}