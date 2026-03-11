using Microsoft.AspNetCore.Mvc;
using DevJobsAPI.Models;
using DevJobsAPI.Dtos.JobPosting;
using DevJobsAPI.Data;
using DevJobsAPI.Mapper;
using Microsoft.EntityFrameworkCore;
namespace DevJobsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostingsController : Controller
    {
        private readonly ApplicationDbContext? _context;

        public JobPostingsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetJobPostings()
        {
            var jobPostings = await _context.JobPostings.ToListAsync();
            var jobPostingDtos = jobPostings.Select(jp => JobPostingMapper.ToDto(jp)).ToList();
            return Ok(jobPostingDtos);

        }
        [HttpPost]
        public async Task<IActionResult> CreateJobPosting(JobPostingDto jobPostingDto)
        {
            var jobPosting = new JobPosting
            {
                Title = jobPostingDto.Title,
                Description = jobPostingDto.Description,
                Company = jobPostingDto.Company,
                Location = jobPostingDto.Location,
                Salary = jobPostingDto.Salary,
                PostedDate = jobPostingDto.PostedDate
            };
            // save to database
            await _context.JobPostings.AddAsync(jobPosting);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJobPostings), new { id = jobPosting.Id }, JobPostingMapper.ToDto(jobPosting));
        }
    }
}