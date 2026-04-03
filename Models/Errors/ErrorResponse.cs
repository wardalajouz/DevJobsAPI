namespace DevJobsAPI.Models.Error
{
    public class ErrorResponse
    {
        // the http status code like (500,404,400)
        public int StatusCode { get; set; }

        // a human readable message for the frontend developer
        public string ErrorMessage { get; set; }= string.Empty;

    }
}
