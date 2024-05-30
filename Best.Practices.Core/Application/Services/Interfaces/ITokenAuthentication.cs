using System.Security.Claims;

namespace Best.Practices.Core.Application.Services.Interfaces
{
    public interface ITokenAuthentication
    {
        string GenerateToken(string apiKey, IEnumerable<Claim> claims, TimeSpan? expirationInMinutes = null);
        string GenerateToken(string apiKey, TimeSpan? expirationInMinutes = null);
    }
}