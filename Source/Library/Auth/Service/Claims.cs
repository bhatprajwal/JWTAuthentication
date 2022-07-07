using Dao.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth;

public class Claims : IClaims
{
    #region Private Member
    #endregion

    #region Constructors
    public Claims()
    { }
    #endregion

    #region Public Functions
    /// <summary>
    /// Generate claim policy for the user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    public async Task<List<Claim>> GetClaims(ApplicationUser user, IList<string> roles)
    {
        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

        foreach (var userRole in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        return authClaims;
    }
    #endregion

    #region Private Functions
    #endregion
}