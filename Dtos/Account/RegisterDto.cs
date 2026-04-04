using System.ComponentModel.DataAnnotations;

namespace DevJobsAPI.Dtos.Account
{
    // we create this so we dont use the AppUser class directly in our controller, and we can add validation attributes to it
    public class RegisterDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string? Password { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
