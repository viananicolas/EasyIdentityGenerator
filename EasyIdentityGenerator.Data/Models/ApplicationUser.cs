using Microsoft.AspNetCore.Identity;

namespace EasyIdentityGenerator.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Password { get; set; }      
    }
}