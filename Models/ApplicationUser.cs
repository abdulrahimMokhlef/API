using Microsoft.AspNetCore.Identity;

namespace API_Project.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
  }
}
