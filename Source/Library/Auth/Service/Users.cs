using Dtos;
using Dao.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth;

public class Users : IUsers
{
    #region Private Member
    private readonly UserManager<ApplicationUser> _userManager;
    #endregion

    #region Constructors
    public Users(
            UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Check is user exist
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<ApplicationUser> IsUserExistAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }    

    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<IdentityResult> CreateUserAsync(Register user)
    {
        ApplicationUser appUser = new()
        {
            Email = user.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = user.Username
        };

        return await _userManager.CreateAsync(appUser, user.Password);
    }

    /// <summary>
    /// Check password for the user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    /// <summary>
    /// Update User details
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<List<ApplicationUser>> GetAllAsync()
    {
        return _userManager.Users.ToList();
    }
    #endregion

    #region Private Functions
    #endregion
}