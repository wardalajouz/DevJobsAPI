using DevJobsAPI.Dtos.Account;
using DevJobsAPI.Interfaces;
using DevJobsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.Security.Claims;

namespace DevJobsAPI.Controllers
{

    [Route("api/account")]
    [ApiController] // This handles automatic validation and cleaner error responses
    public class AccountController : ControllerBase
    { // this is where we inject the usermaneger and the token service, and we will have two endpoints: register and login. Both will return a token if successful, and we will use the same DTO for both, but we will validate the password only for the register endpoint, and for the login endpoint we will just check if the user exists and if the password is correct.

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // [ApiController] handles the ModelState.IsValid check automatically, 
                // but keeping it here is fine for explicit control.
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                var createduser = await _userManager.CreateAsync(appUser, registerDto.Password!);

                if (createduser.Succeeded)
                {
                    // Every new user gets the 'User' role by default
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewUserDto
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = _tokenService.CreateToken(appUser)
                        }); // Return the new user details along with the token

                    }
                    else
                    {
                        return BadRequest(roleResult.Errors);
                    }
                }
                else
                {
                    return BadRequest(createduser.Errors);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(loginDto.UserName!);
            if (user == null)
            {
                return Unauthorized("Invalid username or password!");
            }
            // The 'false' at the end means "Don't lock the account if they fail"
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password!);
            if (!passwordValid)
            {
                return Unauthorized("Invalid username or password!");
            }
            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            }); // Return the user details along with the token

        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            return Ok(new
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName, // This will work now that you added it to the model!
                Initial = !string.IsNullOrEmpty(user.FirstName) ? user.FirstName.Substring(0, 1).ToUpper() : "U"
            });
        }

    }
}
