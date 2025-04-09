using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] UserModel request)
  {
    var user = await _authService.RegisterAsync(request.Email, request.PasswordHash);

    if (user == null)
      return BadRequest("User already exists");

    return Ok("User registered successfully");
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] UserModel request)
  {
    var token = await _authService.LoginAsync(request.Email, request.PasswordHash);

    if (token == null)
      return Unauthorized("Invalid credentials");

    return Ok(new { token });
  }
}