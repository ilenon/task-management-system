using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services;

/// <summary>
/// Interface for authentication service.
/// </summary>
public interface IAuthService
{
  Task<UserModel> RegisterAsync(string email, string password);
  Task<string?> LoginAsync(string email, string password);
}