using Microsoft.AspNetCore.Mvc;
using DevJobsAPI.Models;
using DevJobsAPI.Dtos.JobPosting;
using DevJobsAPI.Mapper;
using DevJobsAPI.Interfaces;
using DevJobsAPI.Helpers;

namespace DevJobsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostingsController : ControllerBase
    {
        private readonly IJobRepository _repository;

        public JobPostingsController(IJobRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
                 [FromQuery] QueryObject query) // we can use the QueryObject class to encapsulate all the query parameters for filtering and pagination, this will help us to keep our controller clean and organized.
        {

            var jobs = await _repository.GetAllAsync(query);

            var jobDtos = jobs.Select(s => s.ToDto()).ToList();

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
        public async Task<IActionResult> Create([FromBody] CreateJobPostingRequestDto createDto) // [FromBody] is optional here since it's the default for complex types, but it's good to be explicit
        {
            var jobModel = new JobPosting
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Company = createDto.Company,
                Location = createDto.Location,
                Salary = createDto.Salary,
                PostedDate = DateTime.Now
            };

            // save to database via repository
            // we use the repository to add the job posting to the database, this will help us to keep our controller clean 
            // and separate the concerns of data access from the controller logic.
            await _repository.CreateAsync(jobModel);

            return CreatedAtAction(nameof(GetById), new { id = jobModel.Id }, jobModel.ToDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // The repository handles finding the entity and the SaveChangesAsync logic
            var job = await _repository.DeleteAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            // return 204 No Content as the professional way to say "It's gone"
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateJobPostingRequestDto updateDto)
        {
            var jobModel = new JobPosting
            {
                Title = updateDto.Title,
                Description = updateDto.Description,
                Company = updateDto.Company,
                Location = updateDto.Location,
                Salary = updateDto.Salary
            };
            var updatedJob = await _repository.UpdateAsync(id, jobModel);
            if (updatedJob == null)
            {
                return NotFound();
            }
            return Ok(updatedJob.ToDto());

        }
    }
}