using Dtos;
using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Sample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    #region Private Member
    private readonly IUsers _user;
    private readonly IRoles _role;
    private readonly IClaims _claims;
    private readonly ITokens _tokens;
    #endregion

    #region Constructor
    public AuthenticateController(
            IUsers user,
            IRoles role,
            IClaims claims,
            ITokens tokens)
    {
        _user = user;
        _role = role;
        _claims = claims;
        _tokens = tokens;
    }
    #endregion

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] Register register)
    {
        var userDetail = await _user.IsUserExistAsync(register.Username);
        if (userDetail != null)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

        var result = await _user.CreateUserAsync(register);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        return Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        var user = await _user.IsUserExistAsync(login.Username);
        if (user != null && await _user.CheckPasswordAsync(user, login.Password))
        {
            var userRoles = await _role.GetRolesAsync(user);

            var authClaims = await _claims.GetClaims(user, userRoles);

            var token = _tokens.CreateToken(authClaims);

            user.RefreshToken = _tokens.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_tokens.GetRefreshTokenValidityInDays());

            await _user.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = user.RefreshToken,
                Expiration = token.ValidTo
            });
        }

        return Unauthorized();
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(Token token)
    {
        if (token is null)
        {
            return BadRequest("Invalid client request");
        }

        string? accessToken = token.AccessToken;
        string? refreshToken = token.RefreshToken;

        var principal = _tokens.GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return BadRequest("Invalid access token or refresh token");
        }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        var user = await _user.IsUserExistAsync(username);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var newAccessToken = _tokens.CreateToken(principal.Claims.ToList());
        var newRefreshToken = _tokens.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _user.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _user.IsUserExistAsync(username);
        if (user == null) return BadRequest("Invalid user name");

        user.RefreshToken = null;
        await _user.UpdateAsync(user);

        return NoContent();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAll()
    {
        var users = await _user.GetAllAsync();
        foreach (var user in users)
        {
            user.RefreshToken = null;
            await _user.UpdateAsync(user);
        }

        return NoContent();
    }
}