using Best.Practices.Core.Application.Services.Interfaces;
using Best.Practices.Core.Common;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Best.Practices.Core.Application.Services
{
    public class TokenAuthentication : ITokenAuthentication
    {
        public string GenerateToken(string apiKey, IEnumerable<Claim> claims, TimeSpan? expirationInMinutes = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var encondingApiKey = Encoding.ASCII.GetBytes(apiKey);

            expirationInMinutes ??= CommonConstants.DefaultApiTokenExpirationInMinutes;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes.Value.TotalMinutes),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encondingApiKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(string apiKey, TimeSpan? expirationInMinutes = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var encondingApiKey = Encoding.ASCII.GetBytes(apiKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes.Value.TotalMinutes),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encondingApiKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
