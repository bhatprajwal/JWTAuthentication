using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth;

public interface ITokens
{
    public JwtSecurityToken CreateToken(List<Claim> authClaims);
    public string GenerateRefreshToken();
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    public int GetRefreshTokenValidityInDays();
}