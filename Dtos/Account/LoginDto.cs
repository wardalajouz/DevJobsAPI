using System.ComponentModel.DataAnnotations;

namespace DevJobsAPI.Dtos.Account
{
    public class LoginDto
    {

        // we need a way for existing users to log in and get a new token without creating a new account, so we will use the same DTO for both registration and login, but we will only validate the password for the registration endpoint, and for the login endpoint we will just check if the user exists and if the password is correct.
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
