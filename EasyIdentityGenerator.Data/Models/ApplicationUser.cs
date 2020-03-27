using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace EasyIdentityGenerator.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string PasswordHashMvc { get; set; }
        [JsonIgnore]
        public string Json { get; internal set; }
        [JsonIgnore]
        public string CoreSql { get; set; }
        [JsonIgnore]
        public string MvcSql { get; set; }
    }
}