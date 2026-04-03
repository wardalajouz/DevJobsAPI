namespace DevJobsAPI.Dtos.Account
{
    public class NewUserDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }  

        public string? Token { get; set; } // we will return this to the client after a successful registration or login, so they can use it for authenticated requests

    }
}
