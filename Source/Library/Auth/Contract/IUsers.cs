using Dtos;
using Dao.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth;

public interface IUsers
{    
    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    public Task<IdentityResult> CreateUserAsync(Register user);
    public Task<ApplicationUser> IsUserExistAsync(string userName);
    public Task<List<ApplicationUser>> GetAllAsync();
    public Task<IdentityResult> UpdateAsync(ApplicationUser user);
}