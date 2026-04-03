namespace DevJobsAPI.Helpers
{
    public class QueryObject
    {
        public string? Title { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public decimal? MinSalary { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10; 


    }
}
