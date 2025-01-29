using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using api.Models;
using api.DTOs;
using api.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _dbContext = dbContext;
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
            return BadRequest(new
            {
                message = "Validation failed.",
                errors = errors
            });
        }

        var user = new User { UserName = registerDto.Email, Email = registerDto.Email, FirstName = registerDto.FirstName, LastName = registerDto.LastName, CreatedAt = DateTime.Now };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (result.Succeeded)
        {
            return Ok(new { message = "User created successfully" });
        }
        return BadRequest(new
        {
            message = "User registration failed.",
            errors = result.Errors.Select(e => e.Description).ToList()
        });
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (loginDto == null)
        {
            return BadRequest(new { message = "Invalid request payload" });
        }
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid Email" });
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Invalid Password." });
        }
        var token = GenerateJwtToken(user, loginDto.RememberMe);
        return Ok(new
        {
            token,
            message = "Login successful"
        });
    }

    private string GenerateJwtToken(User user, bool rememberMe)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique identifier for the token
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenExpiration = rememberMe ? DateTime.Now.AddDays(10) : DateTime.Now.AddHours(1);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: tokenExpiration,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    //An endpoint I created just to test login functionality it gives me the email and roles of the user logged in
    [Authorize]
    [HttpGet("GetUserInfo")]
    public async Task<IActionResult> GetUserInfo()
    {

        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized(new { message = "User not found." });
        }
        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            email = user.Email,
            roles = roles
        });
    }
    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { message = "User not found." });
            }
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Invalid token." });
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expiration = jwtToken.ValidTo;
            var blacklistedToken = new TokenBlacklist { Token = token, Expiration = expiration };

            await _dbContext.TokenBlacklist.AddAsync(blacklistedToken);
            await _dbContext.SaveChangesAsync();

            await _signInManager.SignOutAsync();

            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while logging out.", error = ex.Message });
        }
    }
}