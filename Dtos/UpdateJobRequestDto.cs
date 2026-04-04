// For Updating a Job
public class UpdateJobRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}

// For the Recruiter Dashboard
public class ApplicationResponseDto
{
    public int Id { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string ExperienceLevel { get; set; } = string.Empty;
    public string CVUrl { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; }
}