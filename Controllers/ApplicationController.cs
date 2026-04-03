using DevJobsAPI.Extensions;
using DevJobsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/application")]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationRepository _appRepo;
    private readonly UserManager<AppUser> _userManager;

    public ApplicationController(IApplicationRepository appRepo, UserManager<AppUser> userManager)
    {
        _appRepo = appRepo;
        _userManager = userManager;
    }

    [HttpPost("{jobId}")]
    [Authorize]
    public async Task<IActionResult> Apply(int jobId)
    {
        // 1. Get User ID from the Token
        var username = User.GetUsername(); // Use your existing Claims extension
        var appUser = await _userManager.FindByNameAsync(username);

        // 2. Check if they already applied
        if (await _appRepo.HasUserAppliedAsync(appUser.Id, jobId))
            return BadRequest("You have already applied for this job.");

        // 3. Create the Application
        var application = new JobApplication
        {
            AppUserId = appUser.Id,
            JobPostingId = jobId
        };

        await _appRepo.CreateAsync(application);
        return Ok("Application successful!");
    }
}