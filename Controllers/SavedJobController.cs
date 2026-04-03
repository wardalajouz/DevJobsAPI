using DevJobsAPI.Extensions;
using DevJobsAPI.Interfaces;
using DevJobsAPI.Models;
using DevJobsAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevJobsAPI.Controllers
{
    [Route("api/savedjob")]
    [ApiController]
    public class SavedJobController : ControllerBase
    {
        public readonly UserManager<AppUser> _userManager; //To find the user's details (like their internal ID) using their username.
        public readonly IJobRepository _jobRepository; //To check if a job actually exists before someone tries to save it.
        public readonly ISavedJobRepository _savedJobRepository; // to handle the actual "Saving" and "Deleting" from our bridge table.

        public SavedJobController(UserManager<AppUser> userManager, IJobRepository jobRepository, ISavedJobRepository savedJobRepository)
        {
            _userManager = userManager;
            _jobRepository = jobRepository;
            _savedJobRepository = savedJobRepository;
        }

        [HttpGet]
        [Authorize] // Only logged-in users can see their list
        public async Task<IActionResult> GetUserSavedJobs()
        {
            // use the extension to see who is logged in 
            var username = User.GetUsername();
            var appuser = await _userManager.FindByNameAsync(username);

            // get their jobs from the repo 

            var savedJob = await _savedJobRepository.GetUserSavedJobs(appuser);
            return Ok(savedJob);
        }

        [HttpPost("{jobId:int}")]
        [Authorize]
        public async Task<IActionResult> AddSavedJob(int jobId)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            // first we have to check if the jobid exist

            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null) return BadRequest("Job does not exist");

            // Did they save it already? (Prevent duplicates)
            var userSavedJob = await _savedJobRepository.GetUserSavedJobs(appUser);
            if (userSavedJob.Any(e => e.Id == jobId)) return BadRequest("You already saved this job");


            // create the bridge object 
            var savedJobModel = new SavedJob
            {
                JobId = job.Id,
                AppUserId = appUser.Id
            };

            // save it to the db 
            await _savedJobRepository.CreateAsync(savedJobModel);
            return Created(); // 201 created is professional response
        }


        [HttpDelete("{jobId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteSavedJob(int jobID)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var savedJobs = await _savedJobRepository.GetUserSavedJobs(appUser);

            // first make sure they have the job saved 

            var jobInList= savedJobs.Any(s=> s.Id==jobID);

            if (!jobInList) return BadRequest("Job is not in your saved list");

            await _savedJobRepository.DeleteAsync(appUser,jobID);
            return Ok("Job removed from saved list");


        }

    }
}
