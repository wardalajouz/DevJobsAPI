using System.ComponentModel.DataAnnotations;

namespace DevJobsAPI.Dtos.JobPosting
{
    // data in (request) to create a new job posting
    public class CreateJobPostingRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Company { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        [Range(1, 50000)]
        public decimal Salary { get; set; }
    }
}