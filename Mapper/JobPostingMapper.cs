using DevJobsAPI.Dtos;
using DevJobsAPI.Dtos.JobPosting;
using DevJobsAPI.Models;
namespace DevJobsAPI.Mapper
{
    public class JobPostingMapper
    {
        public static JobPostingDto ToDto(JobPosting jobPosting)
        {
            return new JobPostingDto
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                Description = jobPosting.Description,
                Company = jobPosting.Company,
                Location = jobPosting.Location,
                Salary = jobPosting.Salary,
                PostedDate = jobPosting.PostedDate
            };
        }

    }
}
