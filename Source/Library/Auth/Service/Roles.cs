using Dao.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth;

public class Roles : IRoles
{
    #region Private Member
    private readonly UserManager<ApplicationUser> _userManager;
    #endregion

    #region Constructors
    public Roles(
            UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Get list of roles for the user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }
    #endregion

    #region Private Functions
    #endregion
}