using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_Project.Models;
using API_Project.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace API_Project.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO registerDTO)
    {
      if (await _userManager.FindByNameAsync(registerDTO.Username) != null)
        return BadRequest(new AuthResponseDTO { Message = "Username already exists" });

      var user = new ApplicationUser
      {
        UserName = registerDTO.Username,
        Email = registerDTO.Email,
        FirstName = registerDTO.FirstName,
        LastName = registerDTO.LastName
      };

      var result = await _userManager.CreateAsync(user, registerDTO.Password);

      if (!result.Succeeded)
        return BadRequest(new AuthResponseDTO
        {
          Message = string.Join(", ", result.Errors.Select(e => e.Description))
        });

      await _userManager.AddToRoleAsync(user, "User");

      return Ok(new AuthResponseDTO
      {
        IsAuthenticated = true,
        Message = "User registered successfully",
        Username = user.UserName
      });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDTO)
    {
      var user = await _userManager.FindByNameAsync(loginDTO.Username);

      if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
        return Unauthorized(new AuthResponseDTO { Message = "Invalid username or password" });

      var token = GenerateJwtToken(user);

      return Ok(new AuthResponseDTO
      {
        IsAuthenticated = true,
        Message = "Login successful",
        Token = token,
        Username = user.UserName
      });
    }

    [HttpPost("addrole")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AuthResponseDTO>> AddRole(AddRoleDTO addRoleDTO)
    {
      var user = await _userManager.FindByNameAsync(addRoleDTO.Username);
      if (user == null)
        return NotFound(new AuthResponseDTO { Message = "User not found" });

      if (!await _roleManager.RoleExistsAsync(addRoleDTO.RoleName))
        await _roleManager.CreateAsync(new IdentityRole(addRoleDTO.RoleName));

      var result = await _userManager.AddToRoleAsync(user, addRoleDTO.RoleName);

      if (!result.Succeeded)
        return BadRequest(new AuthResponseDTO
        {
          Message = string.Join(", ", result.Errors.Select(e => e.Description))
        });

      return Ok(new AuthResponseDTO
      {
        IsAuthenticated = true,
        Message = $"Role '{addRoleDTO.RoleName}' added to user '{addRoleDTO.Username}'"
      });
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: _configuration["JwtSettings:Issuer"],
          audience: _configuration["JwtSettings:Audience"],
          claims: new[]
          {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
          },
          expires: DateTime.UtcNow.AddHours(1),
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
