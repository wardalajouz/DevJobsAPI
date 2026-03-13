using DevJobsAPI.Data;
using DevJobsAPI.Interfaces;
using DevJobsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DevJobsAPI.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            // FIX: This was backwards in your code! It should be this:
            _context = context;
        }

        public async Task<List<JobPosting>> GetAllAsync()
        {
            return await _context.JobPostings.ToListAsync();
        }

        public async Task<JobPosting?> GetByIdAsync(int id)
        {
            return await _context.JobPostings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<JobPosting> CreateAsync(JobPosting jobPosting)
        {
            await _context.JobPostings.AddAsync(jobPosting);
            await _context.SaveChangesAsync();
            return jobPosting;
        }

        public async Task<JobPosting?> UpdateAsync(int id, JobPosting jobPosting)
        {
            var existingJob = await _context.JobPostings.FirstOrDefaultAsync(x => x.Id == id);

            if (existingJob == null) return null;

            existingJob.Title = jobPosting.Title;
            existingJob.Description = jobPosting.Description;
            existingJob.Company = jobPosting.Company;
            existingJob.Location = jobPosting.Location;
            existingJob.Salary = jobPosting.Salary;

            await _context.SaveChangesAsync();
            return existingJob;
        }

        public async Task<JobPosting?> DeleteAsync(int id)
        {
            var jobPosting = await _context.JobPostings.FirstOrDefaultAsync(x => x.Id == id);

            if (jobPosting == null) return null;

            _context.JobPostings.Remove(jobPosting);
            await _context.SaveChangesAsync();
            return jobPosting;
        }
    }
}