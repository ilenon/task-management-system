using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services;

public class AuthService : IAuthService
{
  private readonly AppDbContext _context;
  private readonly IConfiguration _configuration;

  public AuthService(AppDbContext context, IConfiguration configuration)
  {
    _context = context;
    _configuration = configuration;
  }

  public async Task<UserModel?> RegisterAsync(string email, string password)
  {
    if(await _context.Users.AnyAsync(u => u.Email == email))
      return null;

    var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

    var user = new UserModel
    {
      Email = email,
      PasswordHash = passwordHash
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return user;
  }

  public async Task<string?> LoginAsync(string email, string password)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    
    if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
      return null;
    
    return GenerateJwtToken(user);
  }

  private string GenerateJwtToken(UserModel user)
  {
    var claims = new[]
    {
      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new Claim(ClaimTypes.Email, user.Email)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: _configuration["Jwt:Issuer"],
      audience: _configuration["Jwt:Audience"],
      claims: claims,
      expires: DateTime.Now.AddMinutes(2),
      signingCredentials: creds);
    
    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}