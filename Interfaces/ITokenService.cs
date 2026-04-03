using DevJobsAPI.Models;

namespace DevJobsAPI.Interfaces
{
    public interface ITokenService
    {
        // This method will generate a JWT token for the authenticated user, which will be used for subsequent requests to protected endpoints. The token will contain claims about the user's identity and roles, allowing the API to authorize access to resources based on the user's permissions.
        string CreateToken(AppUser user);
    }
}
