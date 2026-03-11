using System.ComponentModel.DataAnnotations.Schema;

namespace DevJobsAPI.Models
{
    public class JobPosting
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        // =string.Empty ; is usedto initialize the string properties to an empty string, so that they are not null when we create a new instance of the JobPosting class.

        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;




    }
}
