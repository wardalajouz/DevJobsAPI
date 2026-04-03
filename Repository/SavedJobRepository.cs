using DevJobsAPI.Data;
using DevJobsAPI.Interfaces;
using DevJobsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DevJobsAPI.Repository
{
    public class SavedJobRepository : ISavedJobRepository
    {
        private readonly ApplicationDbContext _context;

        public SavedJobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SavedJob> CreateAsync(SavedJob savedJob)
        {
            await _context.SavedJobs.AddAsync(savedJob);
            await _context.SaveChangesAsync();
            return savedJob;
            
        }

        public async Task<SavedJob> DeleteAsync(AppUser user, int jobId)
        {
            var job =await _context.SavedJobs.FirstOrDefaultAsync(x=> x.AppUserId== user.Id && x.JobId== jobId);
            if (job is null) return null;

             _context.SavedJobs.Remove(job);
            await _context.SaveChangesAsync();
            return job;
            

        }

        public async Task<List<JobPosting>> GetUserSavedJobs(AppUser user)
        {
            return await _context.SavedJobs.Where(u => u.AppUserId==user.Id).Select(job=> new JobPosting
            {
                Id = job.JobId,
                Title = job.JobPosting.Title,
                Company = job.JobPosting.Company,
                Location = job.JobPosting.Location,
                Salary = job.JobPosting.Salary,
                Description = job.JobPosting.Description,
                PostedDate = job.JobPosting.PostedDate
            }).ToListAsync();
        }
    }
}
