using Dao.Models;

namespace Auth;

public interface IRoles
{
    public Task<IList<string>> GetRolesAsync(ApplicationUser user);
}