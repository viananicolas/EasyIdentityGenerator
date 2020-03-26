using Microsoft.AspNetCore.Identity;

namespace EasyIdentityGenerator.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Password { get; set; }
        public string PasswordHashMvc { get; set; }
        public string Json { get; internal set; }
    }
}