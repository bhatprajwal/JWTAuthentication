using Dao.Models;
using System.Security.Claims;

namespace Auth;

public interface IClaims
{
    public Task<List<Claim>> GetClaims(ApplicationUser user, IList<string> roles);
}