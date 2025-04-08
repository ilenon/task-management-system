using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
  /// <summary>
  /// Represents a user of the system.
  /// </summary>
  public class UserModel
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;
  }
}