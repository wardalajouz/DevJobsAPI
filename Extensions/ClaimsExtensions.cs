using System.Security.Claims;
namespace DevJobsAPI.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            // This looks into the JWT "Claims" for the one labeled 'GivenName'
            return user.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.GivenName)).Value;
        }
    }
}
