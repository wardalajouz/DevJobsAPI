using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevJobsAPI.Interfaces;
using DevJobsAPI.Models;
using Microsoft.IdentityModel.Tokens;
namespace DevJobsAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _configuration = config;

            // We convert our secret string into bytes so the encryption engine can use it
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"] ?? throw new InvalidOperationException("JWT:SigningKey is not configured"))) ;

        }
        public string CreateToken(AppUser user)
        {
            // 1. Define the "Claims" (The data inside the passport)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName,user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            // 2. Define the Credentials (The "Signature" method)

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            // 3. Define the Token Descriptor (How long it lasts, who issued it, etc.)

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token is valid for a week
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            // 4. Create the actual token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
