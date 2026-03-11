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
    public class JobPostingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

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
        public async Task<IActionResult> CreateJobPosting([FromBody] CreateJobPostingRequestDto createDto) // [FromBody] is optional here since it's the default for complex types, but it's good to be explicit
        {
            var jobPosting = new JobPosting
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Company = createDto.Company,
                Location = createDto.Location,
                Salary = createDto.Salary,
                PostedDate = DateTime.Now

            };
            // save to database
            await _context.JobPostings.AddAsync(jobPosting);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJobPostings), new { id = jobPosting.Id }, JobPostingMapper.ToDto(jobPosting));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobPostingById([FromRoute] int id) // [Fromroute] is optional here since id is in the route, but it's good to be explicit
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }
            return Ok(JobPostingMapper.ToDto(jobPosting));

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobPostingById([FromRoute] int id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }
            _context.JobPostings.Remove(jobPosting);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobPostingById([FromRoute] int id, [FromBody] CreateJobPostingRequestDto updateDto)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }
            jobPosting.Title = updateDto.Title;
            jobPosting.Description = updateDto.Description;
            jobPosting.Company = updateDto.Company;
            jobPosting.Location = updateDto.Location;
            jobPosting.Salary = updateDto.Salary;
            await _context.SaveChangesAsync();
            return Ok(JobPostingMapper.ToDto(jobPosting));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchJobPostings(
            [FromQuery] string? title,
            [FromQuery] string? company,
            [FromQuery] string? location,
            [FromQuery] decimal? minSalary)
        // this allow the user to search for job posting by these attribute, and the user can provide one or more of these attribute to filter the job posting, if the user provide none of these attribute, it will return all job posting
        {
            var query = _context.JobPostings.AsQueryable(); // Start with the base query
            if (!string.IsNullOrEmpty(title)) // If a title filter is provided, add it to the query
            {
                query = query.Where(jp => jp.Title.ToLower().Contains(title.ToLower())); // This will filter job postings to those whose title contains the specified string (not case-sensitive). 
            }
            if (!string.IsNullOrEmpty(company)) // If a company filter is provided, add it to the query
            {
                query = query.Where(jp => jp.Company.ToLower().Contains(company.ToLower())); // This will filter job postings to those whose company contains the specified string (not case-sensitive). 
            }
            if (!string.IsNullOrEmpty(location))
            {
                query=query.Where(jp => jp.Location.ToLower().Contains(location.ToLower())); // This will filter job postings to those whose location contains the specified string (not case-sensitive).
            }
            if (minSalary.HasValue)
            {
                query=query.Where(jp => jp.Salary >= minSalary.Value);
            }
            var jobPostings = await query.ToListAsync(); // Execute the query and get the results
            var jobPostingDtos = jobPostings.Select(jp => JobPostingMapper.ToDto(jp)).ToList(); // Map the results to DTOs
            return Ok(jobPostingDtos); // Return the filtered list of job postings as DTOs
        }
    }
}