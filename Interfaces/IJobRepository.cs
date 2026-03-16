using DevJobsAPI.Helpers;
using DevJobsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevJobsAPI.Interfaces
{
    public interface IJobRepository
    {

        Task<List<JobPosting>> GetAllAsync(QueryObject query);

        Task<JobPosting?> GetByIdAsync(int id);

        Task<JobPosting> CreateAsync(JobPosting jobPosting);

        Task<JobPosting?> UpdateAsync(int id, JobPosting jobPosting);

        Task<JobPosting?> DeleteAsync(int id);
    }
}
