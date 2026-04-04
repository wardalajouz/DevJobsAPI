using DevJobsAPI.Data;
using DevJobsAPI.Dtos.JobPosting;
using DevJobsAPI.Extensions;
using DevJobsAPI.Helpers;
using DevJobsAPI.Interfaces;
using DevJobsAPI.Mapper;
using DevJobsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DevJobsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostingsController : ControllerBase
    {
        private readonly IJobRepository _repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public JobPostingsController(IJobRepository repository, UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _repository = repository;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
                 [FromQuery] QueryObject query) // we can use the QueryObject class to encapsulate all the query parameters for filtering and pagination, this will help us to keep our controller clean and organized.
        {

            var jobs = await _repository.GetAllAsync(query);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var jobDtos = jobs.Select(s => {
                var dto = s.ToDto();

                if (userId != null)
                {
                    // Fixed: Using 'JobPostingId' to match your model
                    dto.HasApplied = _context.JobApplications.Any(a => a.JobPostingId == s.Id && a.AppUserId == userId);
                    dto.IsSaved = _context.SavedJobs.Any(sj => sj.JobId == s.Id && sj.AppUserId == userId);
                }

                return dto;
            }).ToList();

            return Ok(jobDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id) // [Fromroute] is optional here since id is in the route, but it's good to be explicit
        {
            var job = await _repository.GetByIdAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return Ok(job.ToDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateJobPostingRequestDto createDto) // [FromBody] is optional here since it's the default for complex types, but it's good to be explicit
        {

            // Get the current user from the DB using the Token
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var jobModel = new JobPosting
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Company = createDto.Company,
                Location = createDto.Location,
                Salary = createDto.Salary,
                PostedDate = DateTime.Now,
                AppUserId = appUser.Id // to link the job to this specific user
            };

            // save to database via repository
            // we use the repository to add the job posting to the database, this will help us to keep our controller clean 
            // and separate the concerns of data access from the controller logic.
            await _repository.CreateAsync(jobModel);

            return CreatedAtAction(nameof(GetById), new { id = jobModel.Id }, jobModel.ToDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // find the job first(we need to see who owns it)
            var job = await _repository.GetByIdAsync(id);
            // The repository handles finding the entity and the SaveChangesAsync logic
            //var job = await _repository.DeleteAsync(id);



            if (job == null)
            {
                return NotFound();
            }

            // Get the current user
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            if (job.AppUserId != appUser.Id)
            {
                return Forbid(); // 403 Forbidden - , You can't delete someone else's post
            }
            await _repository.DeleteAsync(id);

            // return 204 No Content as the professional way to say "It's gone"
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateJobPostingRequestDto updateDto)
        {
            // first find the job 
            var job = await _repository.GetByIdAsync(id);
            if (job == null)
            {
                return NotFound("Job posting not found.");
            }

            // then identify the current logged in user 
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            // security check  , do u own this job? 
            if (job.AppUserId != appUser.Id)
            {
                return Forbid();
            }

            var jobModel = new JobPosting
            {
                Title = updateDto.Title,
                Description = updateDto.Description,
                Company = updateDto.Company,
                Location = updateDto.Location,
                Salary = updateDto.Salary
                // we dont update the app user here cuz the owner stays the owner for ever ;)
            };
            var updatedJob = await _repository.UpdateAsync(id, jobModel);

            return Ok(updatedJob.ToDto());

        }

        [HttpPost("{jobId}/apply")]
        [Authorize]
        public async Task<IActionResult> ApplyToJob(int jobId, [FromBody] ApplicationRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if already applied using JobPostingId
            if (await _context.JobApplications.AnyAsync(a => a.JobPostingId == jobId && a.AppUserId == userId))
                return BadRequest("You have already applied for this position.");

            var application = new JobApplication
            {
                JobPostingId = jobId,
                AppUserId = userId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CVUrl = request.CVUrl,
                AppliedAt = DateTime.UtcNow
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Application submitted!" });
        }


        [HttpDelete("{jobId}/unsave")]
        [Authorize]
        public async Task<IActionResult> UnsaveJob(int jobId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Gets current user ID

            var savedJob = await _context.SavedJobs
                .FirstOrDefaultAsync(s => s.JobId == jobId && s.AppUserId == userId);

            if (savedJob == null) return NotFound("Job was not saved.");

            _context.SavedJobs.Remove(savedJob);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}