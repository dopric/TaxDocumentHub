using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaxDocumentHub.Api.Domain;

namespace TaxDocumentHub.Api.Services
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(User user)
        {
            var secretKey = _config["JwtSettings:Secret"]
            ?? throw new InvalidOperationException("JWT Secret key is missing.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims definieren: Welche Infos stecken sicher verpackt im Token?
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"] ?? "TaxDocumentHub",
                audience: _config["JwtSettings:Audience"] ?? "TaxDocumentHubUser",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
