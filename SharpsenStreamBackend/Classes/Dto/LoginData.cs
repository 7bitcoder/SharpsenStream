using System.ComponentModel.DataAnnotations;

namespace SharpsenStreamBackend.Classes.Dto
{
  public class LoginData
  {
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
  }
}
