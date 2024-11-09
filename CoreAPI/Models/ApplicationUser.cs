using Microsoft.AspNetCore.Identity;
namespace CoreAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
    }
}
